using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan;

/// <summary>
/// Singleton class used for managing creation of map representations from provided template and map.
/// 
/// It contains set of all supported map representation representatives. It is main channel between operation on map representations and applications logic (ViewModels/ModelViews).  
/// It implements supporting methods for work with map representations. All map representations should be preferably managed through this singleton.  
/// All operations provided by this class are thread safe as long as same method arguments are not used concurrently multiple times.  
/// </summary>
public class MapRepreManager : 
    IMapGenericVisitor<IMapRepre, (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    IGeoLocatedMapGenericVisitor<IMapRepre, (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepre, IMap, (int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepre, IGeoLocatedMap, (int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
{
    public static MapRepreManager Instance { get; } = new();
    private MapRepreManager() { }

    /// <summary>
    /// Set of representatives of all usable map representations. 
    /// </summary>
    public ISet<IMapRepreRepresentative<IMapRepre>> MapRepreReps { get; } =
        new HashSet<IMapRepreRepresentative<IMapRepre>>() {CompleteNetIntertwiningMapRepreRep.Instance, BlankRepreRep.Instance};
            

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    
    /// <summary>
    /// Method for testing necessity of elevation data for creation of map representation dependent on specific template and map format represented by specific representative.
    /// 
    /// This method should be called before each creation of map representation. Method <c>CreateMapRepre</c> could throw exception, if the wrong overload is called.
    /// </summary>
    /// <param name="template">Template dependency of map representation.</param>
    /// <param name="mapFormat">Map format dependency of map representation.</param>
    /// <param name="mapRepreRep">Representative whose map representation is tested for need for elevation data.</param>
    /// <returns>
    /// Returns necessity of elevation data indication:
    /// - <c>Yes</c>, if there is only elevation dependent constructor
    /// - <c>No</c>, if there is only elevation independent constructor
    /// - <c>NotNecessary</c>, if there are both dependent and independent construcotr
    /// - <c>null</c> if there is no suitable constructor
    /// </returns>
    public NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRepresentative<IMapRepre> mapRepreRep)
    {
        bool dependentFound = false;
        bool independentFound = false;
        foreach (var implementationIndicator in mapRepreRep.ImplementationIndicators)
        {
            if (!dependentFound && implementationIndicator.UsedTemplate == template && implementationIndicator.UsedMapFormat == mapFormat &&
                implementationIndicator.RequiresElevData)
                dependentFound = true;
            else if (!independentFound && implementationIndicator.UsedTemplate == template &&
                     implementationIndicator.UsedMapFormat == mapFormat &&
                     !implementationIndicator.RequiresElevData)
                independentFound = true;
        }
        if (dependentFound)
        {
            if (independentFound) return NeedsElevDataIndic.NotNecessary;
            return NeedsElevDataIndic.Yes;
        }
        if (independentFound) return NeedsElevDataIndic.No;
        return null;
    }

    /// <summary>
    /// This method asks particular representatives of map representation for all of their implementation identifiers and returns their used template and map format combinations.
    /// 
    /// The returned combinations represent usable template and map format combinations for creation of map representations represented by provided representatives. 
    /// </summary>
    /// <param name="mapRepreReps">Provided m.r. representatives whose implementations identifiers are searched.</param>
    /// <returns>Set of template-map format usable combinations.</returns>
    public HashSet<(ITemplate, IMapFormat<IMap>)> GetUsableTemplateMapFormatCombinationsFor(
        ISet<IMapRepreRepresentative<IMapRepre>> mapRepreReps)
    {
        HashSet<(ITemplate, IMapFormat<IMap>)> usableTemplatesMapFormatCombinations = new();
        foreach (IMapRepreRepresentative<IMapRepre> mapRepreRep in mapRepreReps)
        {
            foreach (var constructor in mapRepreRep.ImplementationIndicators)
            {
                usableTemplatesMapFormatCombinations.Add((constructor.UsedTemplate, constructor.UsedMapFormat));
            }
        }
        return usableTemplatesMapFormatCombinations;
    }

    /// <summary>
    /// Calls <see cref="GetUsableTemplateMapFormatCombinationsFor"/> method for all viable map representation representatives.
    /// 
    /// Useful for discovery of all usable template-map format combinations in application.  
    /// For more information on this method see its documentation.  
    /// </summary>
    /// <returns>Set of template-map format usable combinations.</returns>
    public HashSet<(ITemplate, IMapFormat<IMap>)> GetAllUsableTemplateMapFormatCombinations()
    {
        return GetUsableTemplateMapFormatCombinationsFor(MapRepreReps);
    }

    /// <summary>
    /// Returns representatives of all map representation that are together with defined user model type usable in provided searching algorithm.
    /// </summary>
    /// <param name="algorithm">Algorithm for which usable map representations are searched for.</param>
    /// <param name="userModelType">User model type with which represented map representation type must form usable combination in provided searching algorithm.</param>
    /// <returns>Set of representatives of usable map representations.</returns>
    public HashSet<IMapRepreRepresentative<IMapRepre>> GetUsableMapRepreRepsFor(
        ISearchingAlgorithm algorithm, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    {
        HashSet<IMapRepreRepresentative<IMapRepre>> usableMapRepreReps = new();
        foreach (var mapRepreRep in MapRepreReps)
            if(SearchingAlgorithmManager.Instance.GetRelevantGraphCreatorIndexIfRepresentUsableMapRepreUserModelCombFor(mapRepreRep,  userModelType, algorithm) > -1)
                    usableMapRepreReps.Add(mapRepreRep);
        return usableMapRepreReps;
    }

    /// <summary>
    /// Returns representatives of map representations that can be constructed using given template and map format.
    /// </summary>
    /// <param name="template">Requested template for match.</param>
    /// <param name="mapFormat">Requested map format for match.</param>
    /// <returns>Set of representatives of all usable map representations.</returns>
    public HashSet<IMapRepreRepresentative<IMapRepre>> GetUsableMapRepreRepsFor(ITemplate template,
        IMapFormat<IMap> mapFormat)
    {
        var resultingSet = new HashSet<IMapRepreRepresentative<IMapRepre>>();
        foreach (IMapRepreRepresentative<IMapRepre> mapRepreRep in MapRepreReps)
        {
            foreach (var implementationIdentifier in mapRepreRep.ImplementationIndicators)
            {
                if (template == implementationIdentifier.UsedTemplate && mapFormat == implementationIdentifier.UsedMapFormat)
                {
                    resultingSet.Add(mapRepreRep);
                    break;
                }
            }
        }
        return resultingSet;
    }

    /// <summary>
    /// Method which creates map representation for specified representative from provided template and map. This is core method of this manager.
    /// 
    /// It constructs the map representation without requiring elevation data.  
    /// It uses generic visitor pattern on template and map in order to gain their real types as generic parameters.  
    /// Before calling this method it should be checked that given representative contains constructor for provided template and map and it does not require elevation data. Construction may throw exception if provided combination is not usable.  
    /// </summary>
    /// <param name="template">Used template in creation of map representation.</param>
    /// <param name="map">Used map in creation of map representation.</param>
    /// <param name="graphCreatorIndex">Index of graph constructor, which is used for creation of graph reprezentation of map.</param>
    /// <param name="mapRepreRep">Representative of created map representation.</param>
    /// <param name="configuration">Configuration of created map representation. Type of configuration must match with <c>TConfiguration</c> type parameter of graph representative which corresponds to provided map repre. representative.</param>
    /// <param name="constructionProgress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <returns>Created map representation.</returns>
    public IMapRepre CreateMapRepre(ITemplate template, IMap map, int graphCreatorIndex, IMapRepreRepresentative<IMapRepre> mapRepreRep, IConfiguration configuration,
        IProgress<MapRepreConstructionReport>? constructionProgress = null, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric(this, (template, graphCreatorIndex, mapRepreRep, configuration, constructionProgress, cancellationToken));
    }

    IMapRepre IMapGenericVisitor<IMapRepre, (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap map,
        (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (template, graphCreatorIndex, mapRepreRepresentativ, configuration, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepre, TMap, IMap, (int, IMapRepreRepresentative<IMapRepre>, IConfiguration,
                IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, map,
                (graphCreatorIndex, mapRepreRepresentativ, configuration, progress, cancellationToken));
    }
    IMapRepre ITemplateGenericVisitor<IMapRepre, IMap, (int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map, 
        ( int, IMapRepreRepresentative<IMapRepre>, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams) 
    {
        var (graphCreatorIndex, mapRepreRepresentativ, configuration, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, graphCreatorIndex, configuration, progress, cancellationToken);
    }

    /// <summary>
    /// Method which creates for specified representative map representation from provided template and map. This is core method of this manager.
    /// 
    /// For construction of this map representation are required elevation data for provided maps area.  
    /// It uses generic visitor pattern on template and map in order to gain their real types as generic parameters.  
    /// Before calling this method it should be checked that given representative contains constructor for provided template and map and that it requires elevation data. Construction may throw exception if provided combination is not usable.  
    /// </summary>
    /// <param name="template">Used template in creation of map representation.</param>
    /// <param name="map">Used map in creation of map representation.</param>
    /// <param name="graphCreatorIndex">Index of graph constructor, which is used for creation of graph reprezentation of map.</param>
    /// <param name="mapRepreRep">Representative of created map representation.</param>
    /// <param name="elevData">Elevation data corresponding to map area used in map representation creation.</param>
    /// <param name="configuration">Configuration of created map representation. Type of configuration must match with <c>TConfiguration</c> type parameter of graph representative which corresponds to provided map repre. representative.</param>
    /// <param name="constructionProgress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <returns>Created map representation.</returns>
    public IMapRepre CreateMapRepre(ITemplate template, IGeoLocatedMap map, int graphCreatorIndex, IMapRepreRepresentative<IMapRepre> mapRepreRep, IElevData elevData, IConfiguration configuration, IProgress<MapRepreConstructionReport>? constructionProgress = null, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric(this, (template, graphCreatorIndex, mapRepreRep, elevData, configuration, constructionProgress, cancellationToken));

    }
    IMapRepre IGeoLocatedMapGenericVisitor<IMapRepre, (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap geoLocatedMap,
        (ITemplate, int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (template, graphCreatorIndex, mapRepreRepresentativ, elevData, configuration, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepre, TMap, IGeoLocatedMap, (int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, 
                IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, geoLocatedMap, 
                (graphCreatorIndex, mapRepreRepresentativ, elevData, configuration, progress, cancellationToken));
    }

    IMapRepre ITemplateGenericVisitor<IMapRepre, IGeoLocatedMap, (int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration,
            IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map,
            (int, IMapRepreRepresentative<IMapRepre>, IElevData, IConfiguration, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (graphCreatorIndex, mapRepreRepresentativ, elevData, configuration, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, graphCreatorIndex,
            elevData, configuration, progress, cancellationToken);
    }
}

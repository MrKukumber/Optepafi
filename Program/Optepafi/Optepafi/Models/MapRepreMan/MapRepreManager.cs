using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

/// <summary>
/// Singleton class used for managing creation of map representations from provided template and map. It contains set of all supported map representation representatives. It is main channel between operation on map representations and applications logic (ViewModels/ModelViews).
/// It implements supporting methods for work with map representations. All map representations should be preferably managed through this singleton.
/// All operations provided by this class are thread safe as long as same method arguments are not used concurrently multiple times.
/// </summary>
public class MapRepreManager : 
    IMapGenericVisitor<IMapRepre, (ITemplate, IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?)>,
    IMapGenericVisitor<IMapRepre, (ITemplate, IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepre, IMap, (IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepre, IMap, (IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?)>
{
    public static MapRepreManager Instance { get; } = new();
    private MapRepreManager() { }

    /// <summary>
    /// Set of representatives of all usable map representations. 
    /// </summary>
    private ISet<IMapRepreRepresentative<IMapRepre>> MapRepreReps { get; } =
        new HashSet<IMapRepreRepresentative<IMapRepre>>();
            

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given template, mapFormat and mapRepreRep
    /// <summary>
    /// Method for testing necessity of elevation data for creation of map representation dependent on specific template and map format represented by specific representative.
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
    /// - <c>null</c> if there is no sutable constructor
    /// </returns>
    public NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRepresentative<IMapRepre> mapRepreRep)
    {
        bool dependentFound = false;
        bool independentFound = false;
        foreach (var implementationInfo in mapRepreRep.ImplementationIdentifiers)
        {
            if (!dependentFound && implementationInfo.UsedTemplate == template && implementationInfo.UsedMapFormat == mapFormat &&
                implementationInfo.RequiresElevData)
                dependentFound = true;
            else if (!independentFound && implementationInfo.UsedTemplate == template &&
                     implementationInfo.UsedMapFormat == mapFormat &&
                     !implementationInfo.RequiresElevData)
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

    public HashSet<(ITemplate, IMapFormat<IMap>)> GetUsableTemplateMapFormatCombinationsFor(
        ISet<IMapRepreRepresentative<IMapRepre>> mapRepreReps)
    {
        HashSet<(ITemplate, IMapFormat<IMap>)> usableTemplatesMapFormatCombinations = new();
        foreach (IMapRepreRepresentative<IMapRepre> mapRepreRep in mapRepreReps)
        {
            foreach (var constructor in mapRepreRep.ImplementationIdentifiers)
            {
                usableTemplatesMapFormatCombinations.Add((constructor.UsedTemplate, constructor.UsedMapFormat));
            }
        }
        return usableTemplatesMapFormatCombinations;
    }

    public HashSet<(ITemplate, IMapFormat<IMap>)> GetAllUsableTemplateMapFormatCombinations()
    {
        return GetUsableTemplateMapFormatCombinationsFor(MapRepreReps);
    }

    public HashSet<IMapRepreRepresentative<IMapRepre>> GetUsableMapRepreRepsFor(
        ISearchingAlgorithm algorithm)
    {
        HashSet<IMapRepreRepresentative<IMapRepre>> usableMapRepreReps = new();
        foreach (var mapRepreRep in MapRepreReps)
        {
            if (algorithm.DoesRepresentUsableMapRepre(mapRepreRep)) usableMapRepreReps.Add(mapRepreRep);
        }

        return usableMapRepreReps;
    }

    public HashSet<IMapRepreRepresentative<IMapRepre>> GetUsableMapRepreRepsFor(ITemplate template,
        IMapFormat<IMap> mapFormat)
    {
        var resultingSet = new HashSet<IMapRepreRepresentative<IMapRepre>>();
        foreach (IMapRepreRepresentative<IMapRepre> mapRepreRep in MapRepreReps)
        {
            foreach (var implementationInfo in mapRepreRep.ImplementationIdentifiers)
            {
                if (template == implementationInfo.UsedTemplate && mapFormat == implementationInfo.UsedMapFormat)
                {
                    resultingSet.Add(mapRepreRep);
                    break;
                }
            }
        }
        return resultingSet;
    }
    
    

    // Creates map representation without using elevation data.
    // Throws exception, if there is no constructor not using elevation data for creating map repre from template and map
    public IMapRepre CreateMapRepre(ITemplate template, IMap map, IMapRepreRepresentative<IMapRepre> mapRepreRep,
        IProgress<GraphCreationReport>? constructionProgress = null, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric<IMapRepre, 
                (ITemplate, IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?)>
                (this, (template, mapRepreRep, constructionProgress, cancellationToken));
    }

    IMapRepre IMapGenericVisitor<IMapRepre, (ITemplate, IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap map,
        (ITemplate, IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?) otherParams)
    {
        var (template, mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepre, TMap, IMap, (IMapRepreRepresentative<IMapRepre>,
                IProgress<GraphCreationReport>?, CancellationToken?)>(this, map,
                (mapRepreRepresentativ, progress, cancellationToken));
    }
    IMapRepre ITemplateGenericVisitor<IMapRepre, IMap, (IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map, 
        (IMapRepreRepresentative<IMapRepre>, IProgress<GraphCreationReport>?, CancellationToken?) otherParams) 
    {
        var (mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, progress, cancellationToken);
    }

    


    // Creates map representation by using elevation data.
    // Throws exception, if there is no constructor using elevation data for creating map repre from template and map
    public IMapRepre CreateMapRepre(ITemplate template, IGeoLocatedMap map, IMapRepreRepresentative<IMapRepre> mapRepreRep, IElevData elevData, IProgress<GraphCreationReport>? constructionProgress = null, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric<IMapRepre, 
                (ITemplate, IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?)>
                (this, (template, mapRepreRep, elevData, constructionProgress, cancellationToken));

    }

    IMapRepre IMapGenericVisitor<IMapRepre, (ITemplate, IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap map,
        (ITemplate, IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?) otherParams)
    {
        var (template, mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepre, TMap, IMap, (IMapRepreRepresentative<IMapRepre>, IElevData, 
                IProgress<GraphCreationReport>?, CancellationToken?)>(this, map,
                (mapRepreRepresentativ, elevData, progress, cancellationToken));
    }
    IMapRepre ITemplateGenericVisitor<IMapRepre, IMap, (IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map, 
        (IMapRepreRepresentative<IMapRepre>, IElevData, IProgress<GraphCreationReport>?, CancellationToken?) otherParams) 
    {
        var (mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, elevData, progress, cancellationToken);
    }
}

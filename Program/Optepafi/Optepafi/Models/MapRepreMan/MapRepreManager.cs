using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public class MapRepreManager : 
    IMapGenericVisitor<IMapRepresentation?, (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    IMapGenericVisitor<IMapRepresentation?, (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepresentation?, IMap, (IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepresentation?, IMap, (IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
{
    public static MapRepreManager Instance { get; } = new();
    private MapRepreManager() { }

    private ISet<IMapRepreRepresentativ<IMapRepresentation>> MapRepreReps { get; } =
        new HashSet<IMapRepreRepresentativ<IMapRepresentation>>(); //TODO: premysliet jak to rperezentovat, mozno by bolo lepsie aby to bolo usporiadane v nejakom liste
            

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given templateRep, mapFormat and mapRepreRep
    public NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep)
    {
        bool dependentFound = false;
        bool independentFound = false;
        foreach (var constructor in mapRepreRep.MapRepreConstrs)
        {
            if (!dependentFound && constructor.UsedTemplate == template && constructor.UsedMapFormat == mapFormat &&
                constructor.RequiresElevData)
                dependentFound = true;
            else if (!independentFound && constructor.UsedTemplate == template &&
                     constructor.UsedMapFormat == mapFormat &&
                     !constructor.RequiresElevData)
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
        ISet<IMapRepreRepresentativ<IMapRepresentation>> mapRepreReps)
    {
        HashSet<(ITemplate, IMapFormat<IMap>)> usableTemplatesMapFormatCombinations = new();
        foreach (IMapRepreRepresentativ<IMapRepresentation> mapRepreRep in mapRepreReps)
        {
            foreach (var constructor in mapRepreRep.MapRepreConstrs)
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

    public HashSet<IMapRepreRepresentativ<IMapRepresentation>> GetUsableMapRepreRepsFor(
        ISearchingAlgorithm algorithm)
    {
        HashSet<IMapRepreRepresentativ<IMapRepresentation>> usableMapRepreReps = new();
        foreach (var mapRepreRep in MapRepreReps)
        {
            if (algorithm.DoesRepresentUsableMapRepre(mapRepreRep)) usableMapRepreReps.Add(mapRepreRep);
        }

        return usableMapRepreReps;
    }
    
    

    // Creates map representation without using elevation data.
    // Returns null, if there is no constructor not using elevation data for creating map repre from template and map
    public IMapRepresentation? CreateMapRepre(ITemplate template, IGeoReferencedMap map, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep, 
        IProgress<MapRepreConstructionReport>? constructionProgress = null, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric<IMapRepresentation?, 
                (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
                (this, (template, mapRepreRep, constructionProgress, cancellationToken));
    }

    IMapRepresentation? IMapGenericVisitor<IMapRepresentation?, (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap map,
        (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (template, mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepresentation?, TMap, IMap, (IMapRepreRepresentativ<IMapRepresentation>,
                IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, map,
                (mapRepreRepresentativ, progress, cancellationToken));
    }
    IMapRepresentation? ITemplateGenericVisitor<IMapRepresentation?, IMap, (IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map, 
        (IMapRepreRepresentativ<IMapRepresentation>, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams) 
    {
        var (mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, progress, cancellationToken);
    }

    


    // Creates map representation by using elevation data.
    // Returns null, if there is no constructor using elevation data for creating map repre from template and map
    public IMapRepresentation? CreateMapRepre(ITemplate template, IGeoReferencedMap map, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep, IElevData elevData, IProgress<MapRepreConstructionReport>? constructionProgress, CancellationToken? cancellationToken)
    {
        return map.AcceptGeneric<IMapRepresentation?, 
                (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
                (this, (template, mapRepreRep, elevData, constructionProgress, cancellationToken));
    }

    IMapRepresentation? IMapGenericVisitor<IMapRepresentation?, (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap>(TMap map,
        (ITemplate, IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (template, mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return template
            .AcceptGeneric<IMapRepresentation?, TMap, IMap, (IMapRepreRepresentativ<IMapRepresentation>, IElevData, 
                IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, map,
                (mapRepreRepresentativ, elevData, progress, cancellationToken));
    }
    IMapRepresentation? ITemplateGenericVisitor<IMapRepresentation?, IMap, (IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TMap>(TTemplate template, TMap map, 
        (IMapRepreRepresentativ<IMapRepresentation>, IElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams) 
    {
        var (mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return mapRepreRepresentativ.CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(template, map, elevData, progress, cancellationToken);
    }
}

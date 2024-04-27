using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public class MapRepreManager : ITemplateGenericVisitor<IMapRepresentation<ITemplate>?, (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IMapRepresentation<ITemplate>?, (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    IMapGenericVisitorWithSomeone<IMapRepresentation<ITemplate>?, ITemplate,(IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>,
    IMapGenericVisitorWithSomeone<IMapRepresentation<ITemplate>?, ITemplate, (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
{
    public static MapRepreManager Instance { get; } = new();
    private MapRepreManager() { }

    public IReadOnlySet<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>> GraphRepres { get; } = 
        ImmutableHashSet.Create<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>>(/*TODO: doplnit mapRepresAgents*/);
            

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given templateRep, mapFormat and mapRepreRep
    public NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep)
    {
        bool dependentFound = false;
        bool independentFound = false;
        foreach (IMapRepreConstructor<ITemplate, IMap, IMapRepresentation<ITemplate>> constructor in mapRepreRep.MapRepreConstrs)
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

    public HashSet<(ITemplate, IMapFormat<IMap>)> UsableTemplateMapFormatCombinationsFor(
        IReadOnlySet<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>> mapRepreReps)
    {
        HashSet<(ITemplate, IMapFormat<IMap>)> usableTemplatesMapFormatCombinations = new();
        foreach (IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep in mapRepreReps)
        {
            foreach (var constructor in mapRepreRep.MapRepreConstrs)
            {
                usableTemplatesMapFormatCombinations.Add((constructor.UsedTemplate, constructor.UsedMapFormat));
            }
        }
        return usableTemplatesMapFormatCombinations;
    }

    public HashSet<(ITemplate, IMapFormat<IMap>)> AllUsableTemplateMapFormatCombinations()
    {
        return UsableTemplateMapFormatCombinationsFor(GraphRepres);
    }

    // Creates map representation without using elevation data.
    // Returns null, if there is no constructor not using elevation data for creating map repre from template and map
    public IMapRepresentation<ITemplate>? CreateMapRepre(ITemplate template, IMap map, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, IProgress<MapRepreConstructionReport>? constructionProgress, CancellationToken? cancellationToken)
    {
        return template.AcceptGeneric<IMapRepresentation<ITemplate>?, 
                (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
                (this, (map, mapRepreRep, constructionProgress, cancellationToken));
    }

    IMapRepresentation<ITemplate>? ITemplateGenericVisitor<IMapRepresentation<ITemplate>?, (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate>(TTemplate template,
        (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams) 
    {
        var (map, mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return map.AcceptGenericWithSomeone<IMapRepresentation<ITemplate>?, TTemplate, ITemplate, (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, template, (mapRepreRepresentativ, progress, cancellationToken));
    }

    IMapRepresentation<ITemplate>? IMapGenericVisitorWithSomeone<IMapRepresentation<ITemplate>?, ITemplate,(IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap, TSomeone>(TMap map, TSomeone someone,
        (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (mapRepreRepresentativ, progress, cancellationToken) = otherParams;
        return (IMapRepresentation<ITemplate>?) mapRepreRepresentativ.CreateMapRepre(someone, map, progress, cancellationToken);
    }
    


    // Creates map representation by using elevation data.
    // Returns null, if there is no constructor using elevation data for creating map repre from template and map
    public IMapRepresentation<ITemplate>? CreateMapRepre(ITemplate template, IMap map, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, ElevData elevData, IProgress<MapRepreConstructionReport>? constructionProgress, CancellationToken? cancellationToken)
    {
        return template.AcceptGeneric<IMapRepresentation<ITemplate>?, 
                (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
                (this, (map, mapRepreRep, elevData, constructionProgress, cancellationToken));
    }
    IMapRepresentation<ITemplate>? ITemplateGenericVisitor<IMapRepresentation<ITemplate>?, (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TTemplate>(TTemplate template,
        (IMap, IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams) 
    {
        var (map, mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return map.AcceptGenericWithSomeone<IMapRepresentation<ITemplate>?, TTemplate, ITemplate, (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>(this, template, (mapRepreRepresentativ, elevData, progress, cancellationToken));
    }
    IMapRepresentation<ITemplate>? IMapGenericVisitorWithSomeone<IMapRepresentation<ITemplate>?, ITemplate,(IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?)>
        .GenericVisit<TMap, TSomeone>(TMap map, TSomeone someone,
        (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>, ElevData, IProgress<MapRepreConstructionReport>?, CancellationToken?) otherParams)
    {
        var (mapRepreRepresentativ, elevData, progress, cancellationToken) = otherParams;
        return (IMapRepresentation<ITemplate>?) mapRepreRepresentativ.CreateMapRepre(someone, map, elevData, progress, cancellationToken);
    }
}

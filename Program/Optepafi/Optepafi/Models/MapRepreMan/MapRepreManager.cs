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

public static class MapRepreManager
{
    public static bool PreferUsingElevData;

    public static ISet<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>> GraphRepres { get; } = 
        ImmutableHashSet.Create<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>>(/*TODO: doplnit mapRepresAgents*/);
            

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given templateRep, mapFormat and mapRepreRep
    public static NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep)
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

    public static HashSet<(ITemplate, IMapFormat<IMap>)> UsableTemplateMapFormatCombinationsFor(
        ISet<IMapRepreRepresentativ<IMapRepresentation<ITemplate>>> mapRepreReps)
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

    public static HashSet<(ITemplate, IMapFormat<IMap>)> AllUsableTemplateMapFormatCombinations()
    {
        return UsableTemplateMapFormatCombinationsFor(GraphRepres);
    }

    // Creates map representation without using elevation data.
    // Returns null, if there is no constructor not using elevation data for creating map repre from template and map
    public static IMapRepresentation<ITemplate>? CreateMapRepre(ITemplate template, IMap map, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, IProgress<MapRepreConstructionReport>? constructionProgress, CancellationToken? cancellationToken)
    {
        return template.VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(mapRepreRep, map, constructionProgress, cancellationToken);
    }

    // Creates map representation by using elevation data.
    // Returns null, if there is no constructor using elevation data for creating map repre from template and map
    public static IMapRepresentation<ITemplate>? CreateMapRepre(ITemplate template, IMap map, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, ElevData elevData, IProgress<MapRepreConstructionReport>? constructionProgress, CancellationToken? cancellationToken)
    {
        return template.VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(mapRepreRep, map, elevData, constructionProgress, cancellationToken);
    }
}

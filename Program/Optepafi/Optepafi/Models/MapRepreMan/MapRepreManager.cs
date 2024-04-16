using System;
using System.Collections.Generic;
using Avalonia.Controls.Templates;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public static class MapRepreManager
{
    public static bool PreferUsingElevData;
    public static IMapRepreRep<IMapRepre>[] GraphRepres { get; } = { /*TODO: doplnit mapRepresAgents*/ };

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given templateRep, mapFormat and mapRepreRep
    public static NeedsElevDataIndic? DoesNeedElevData(ITemplate template, IMapFormat<IMap> mapFormat, IMapRepreRep<IMapRepre> mapRepreRep)
    {
        
    }

    public static HashSet<(ITemplate, IMapFormat<IMap>)> UsableTemplatesAndMapFormatsCombinationsFor(
        HashSet<IMapRepreRep<IMapRepre>> mapRepreReps)
    {
        foreach (IMapRepreRep<IMapRepre> mapRepreRep in mapRepreReps)
        {
            
        }
    }

    // Creates map representation without using elevation data.
    // Returns null, if there is no constructor not using elevation data for creating map repre from template and map
    public static IMapRepre? CreateMapRepre(ITemplate template, IMap map, IMapRepreRep<IMapRepre> mapRepreRep)
    {
        template.VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(mapRepreRep, map);
    }

    // Creates map representation by using elevation data.
    // Returns null, if there is no constructor using elevation data for creating map repre from template and map
    public static IMapRepre? CreateMapRepre(ITemplate template, IMap map, IMapRepreRep<IMapRepre> mapRepreRep, ElevData elevData)
    {
        template.VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(mapRepreRep, map, elevData);
    }
}

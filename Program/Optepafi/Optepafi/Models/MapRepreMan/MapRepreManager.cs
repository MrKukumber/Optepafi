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
    public static IMapRepreRep<MapRepre>[] GraphRepres { get; } = { /*TODO: doplnit mapRepresAgents*/ };

    public enum NeedsElevDataIndic { Yes, NotNecessary, No };
    //returns null, if there is no constructor for given templateRep, mapFormat and mapRepreRep
    public static NeedsElevDataIndic? DoesNeedElevData(ITemplateRep<Template> templateRep, IMapFormat<Map> mapFormat, IMapRepreRep<MapRepre> mapRepreRep)
    {
        
    }

    public static HashSet<(ITemplateRep<Template>, IMapFormat<Map>)> UsableTemplateRepsAndMapFormatsCombinationsFor(
        HashSet<IMapRepreRep<MapRepre>> mapRepreReps)
    {
        foreach (IMapRepreRep<MapRepre> mapRepreRep in mapRepreReps)
        {
            
        }
    }

    // Creates map representation without using elevation data.
    // Returns null, if there is no constructor not using elevation data for creating map repre from template and map
    public static MapRepre? CreateMapRepre(Template template, Map map, IMapRepreRep<MapRepre> mapRepreRep)
    {
        return mapRepreRep.CreateMapRepre(template.TemplateRep.CastTemplate(template)!, map.MapFormat.CastMap(map)!);
    }

    // Creates map representation by using elevation data.
    // Returns null, if there is no constructor using elevation data for creating map repre from template and map
    public static MapRepre? CreateMapRepre(Template template, Map map, IMapRepreRep<MapRepre> mapRepreRep, ElevData elevData)
    {
        return mapRepreRep.CreateMapRepre(template.TemplateRep.CastTemplate(template)!, map.MapFormat.CastMap(map)!, elevData);
    }
}

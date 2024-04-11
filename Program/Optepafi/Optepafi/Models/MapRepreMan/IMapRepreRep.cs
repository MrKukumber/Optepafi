using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepreRep<TMapRepre> where TMapRepre : MapRepre
{
    string MapRepreName { get; }
    IElevDataDependentConstr<Template, Map, MapRepre>[] MapRepreElevDataDependentConstrs { get; }
    IElevDataIndependentConstr<Template, Map, MapRepre>[] MapRepreElevDataIndependentConstrs { get; }

    TMapRepre? CastMapRepre(MapRepre mapRepre)
    {
        if (mapRepre is TMapRepre cMapRepre) return cMapRepre;
        return null;
    }

    TMapRepre? CreateMapRepre(Template template, Map map)
    {
        foreach (var constructor in MapRepreElevDataIndependentConstrs)
        {
            if(constructor is IElevDataIndependentConstr<Orienteering,Map,TMapRepre>(Orienteering, var mapFormat) c
               && mapFormat == map.MapFormat)
            {
                return c.ConstructMapRepre((Orienteering)template, map);
            }
            
        }
    }

    TMapRepre? CreateMapRepre(Template template, Map map, ElevData elevData)
    {
        
    }
}
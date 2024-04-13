using Avalonia.Markup.Xaml.Templates;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepres.ObjectRepres;
using Optepafi.Models.TemplateMan.Templates;
using Template = Optepafi.Models.TemplateMan.Template;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepreRep<out TMapRepre> where TMapRepre : MapRepre
{
    string MapRepreName { get; }

    IElevDataDependentConstr<Template, Map, MapRepre>[] MapRepreElevDataDependentConstrs { get; }

    IElevDataIndependentConstr<Template, Map, MapRepre>[] MapRepreElevDataIndependentConstrs { get; }

    TMapRepre? CastMapRepre(MapRepre mapRepre)
    {
        if (mapRepre is TMapRepre cMapRepre) return cMapRepre;
        return null;
    }

    TMapRepre? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map) where TTemplate : Template where TMap : Map
    {
        foreach (var constructor in MapRepreElevDataIndependentConstrs)
        {
            if(constructor is IElevDataIndependentConstr<TTemplate,TMap,TMapRepre> c)
            {
                return c.ConstructMapRepre(template, map);
            }
        }
        return null;
    }

    TMapRepre? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map, ElevData elevData) where TTemplate : Template where TMap : Map
    {
        foreach (var constructor in MapRepreElevDataDependentConstrs)
        {
            if (constructor is IElevDataDependentConstr<TTemplate, TMap, TMapRepre> c)
            {
                return c.ConstructMapRepre(template, map, elevData);
            }
        }
        return null;
    }
}
/*
    TMapRepre? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map) where TTemplate : Template where TMap : Map
    {
        IElevDataIndependentConstr<TTemplate, TMap, TMapRepre>? constructor =
            GetMapRepreElevDataIndependentConstr<TTemplate, TMap, IElevDataIndependentConstr<TTemplate, TMap, TMapRepre>>();
        if (constructor is not null) 
            return constructor.ConstructMapRepre(template, map);
        return null;
    }

    TMapRepre? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map, ElevData elevData) where TTemplate : Template where TMap : Map
    {
        IElevDataDependentConstr<TTemplate, TMap, TMapRepre>? constructor =
            GetMapRepreElevDataDependentConstr<TTemplate, TMap, IElevDataDependentConstr<TTemplate, TMap, TMapRepre>>();
        if (constructor is not null) 
            return constructor.ConstructMapRepre(template, map, elevData);
        return null;
    }

    TMapRepreConstr? GetMapRepreElevDataDependentConstr<TTemplate, TMap, TMapRepreConstr>()
        where TTemplate : Template
        where TMap : Map
        where TMapRepreConstr : IElevDataDependentConstr<TTemplate, TMap, MapRepre>;

    TMapRepreConstr? GetMapRepreElevDataIndependentConstr<TTemplate, TMap, TMapRepreConstr>()
        where TTemplate : Template
        where TMap : Map
        where TMapRepreConstr : IElevDataIndependentConstr<TTemplate, TMap, MapRepre>;
*/
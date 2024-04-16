using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepreRep<out TMapRepre> where TMapRepre : IMapRepre
{
    string MapRepreName { get; }

    IMapRepreConstr<ITemplate, IMap, TMapRepre>[] MapRepreConstrs { get; }

    TMapRepre? CreateMapRepre<TTemplate, TMap>(TTemplate templateType, TMap map) where TTemplate : ITemplate where TMap : IMap
    {
        foreach (var constructor in MapRepreConstrs)
        {
            if(constructor is IElevDataIndependentConstr<TTemplate,TMap,TMapRepre> c)
            {
                return c.ConstructMapRepre(templateType, map);
            }
        }
        return default;
    }

    TMapRepre? CreateMapRepre<TTemplateType, TMap>(TTemplateType templateType, TMap map, ElevData elevData) where TTemplateType : ITemplate where TMap : IMap
    {
        foreach (var constructor in MapRepreConstrs)
        {
            if (constructor is IElevDataDependentConstr<TTemplateType, TMap, TMapRepre> c)
            {
                return c.ConstructMapRepre(templateType, map, elevData);
            }
        }
        return default;
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
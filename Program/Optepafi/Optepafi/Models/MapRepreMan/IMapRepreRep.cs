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
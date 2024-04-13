using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepres.ObjectRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public abstract class ObjectRepre : MapRepre
{
    //TODO: implement IMapRepre interface
    public IElevDataDependentConstr<Template, Map, MapRepre>[] mapRepreElevDataDependentConstr { get; } = { ObjectRepreOrienteeringOmapConstr.Instance };
    
    TMapRepreConstr? GetMapRepreElevDataDependentConstr<TTemplate, TMap, TMapRepreConstr>()
        where TTemplate : Template
        where TMap : Map
        where TMapRepreConstr : IElevDataDependentConstr<TTemplate, TMap, MapRepre>
    {
        if (ObjectRepreOrienteeringOmapConstr.Instance is TMapRepreConstr mapRepreConstructor)
            return mapRepreConstructor;
        return default;
    }
    TMapRepreConstr? GetMapRepreElevDataIndependentConstr<TTemplate, TMap, TMapRepreConstr>()
        where TTemplate : Template
        where TMap : Map
        where TMapRepreConstr : IElevDataIndependentConstr<TTemplate, TMap, MapRepre>
    {
        return default;
    }
}
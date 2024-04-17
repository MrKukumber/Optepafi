using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public interface IElevDataIndependentConstr<TTemplate, TMap, out TMapRepre> : IMapRepreConstr<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IMapRepre
{
    public TMapRepre? ConstructMapRepre(TTemplate templateType, TMap map);
}
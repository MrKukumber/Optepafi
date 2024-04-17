using Avalonia.Controls;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public interface IElevDataDependentConstr<TTemplateType, TMap, out TMapRepre> : IMapRepreConstr<TTemplateType, TMap, TMapRepre> 
    where TTemplateType : ITemplate where TMap : IMap where TMapRepre : IMapRepre
{
    public TMapRepre? ConstructMapRepre(TTemplateType templateType, TMap map, ElevData elevData);
}
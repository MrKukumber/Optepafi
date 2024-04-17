using System;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepreConstr< out TTemplate, out TMap, out TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IMapRepre
{
    public TTemplate UsedTemplateType { get; }
    public IMapFormat<TMap> UsedMapFormat { get; }
}
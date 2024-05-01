using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IConstructableMapRepre<TTemplate, TMap> : IMapRepresentation where TTemplate : ITemplate where TMap : IMap
{
    
}
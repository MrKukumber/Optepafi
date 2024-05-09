using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public interface IMapRepreElevDataDepImplementationRep<TTemplate, TMap, TMapRepre> :
    IMapRepreSourceIndic<TTemplate, TMap, TMapRepre>, IElevDataIndependentConstr<TTemplate, TMap, TMapRepre>
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IMapRepresentation
{
    
}
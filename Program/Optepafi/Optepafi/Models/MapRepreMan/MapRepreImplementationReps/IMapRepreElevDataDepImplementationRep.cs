using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public interface IMapRepreElevDataDepImplementationRep<TTemplate, TMap, TMapRepre, TVertexAttributes, TEdgeAttributes> :
    IMapRepreSourceIndic<TTemplate, TMap, TMapRepre>, IElevDataDependentConstr<TTemplate, TMap, TMapRepre>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
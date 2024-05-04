using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IMapRepreFunctionality<out TTemplate, out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
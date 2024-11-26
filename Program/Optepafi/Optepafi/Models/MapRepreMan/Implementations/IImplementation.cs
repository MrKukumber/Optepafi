using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Implementations;

//TODO: comment
public interface IImplementation<out TVertex, out TEdge, TVertexAttributes, TEdgeAttributes> : IGraph<TVertex, TEdge>
    where TVertex : IAttributeBearingVertex<TVertexAttributes>
    where TEdge : IAttributesBearingEdge<TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
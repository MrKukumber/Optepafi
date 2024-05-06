using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VertecesAndEdges;

public interface IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> :
    IPredecessorRememberingVertex<IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

public interface IPredecessorRememberingVertex<TBasicEdge, TVertexAttributes> : 
    IBasicVertex<TBasicEdge, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TBasicEdge : IEdge
{
    IPredecessorRememberingVertex<TBasicEdge, TVertexAttributes>? Predecessor { get; set; }
}

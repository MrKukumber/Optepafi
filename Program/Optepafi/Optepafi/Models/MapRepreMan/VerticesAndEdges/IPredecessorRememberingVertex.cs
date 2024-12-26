using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VertecesAndEdges;

/// <summary>
/// Predecessor remembering vertex whose type of hold edges is assign to be of type <see cref="IBasicEdge{TVertex,TEdgeAttributes}"/>.
/// 
/// This is just auxiliary, quality of life improving interface.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of borne vertex attributes.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes borne by assigned type of hold edges.</typeparam>
// public interface IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> :
    // IPredecessorRememberingVertex<
        // IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>,
        // TVertexAttributes>
    // where TVertexAttributes : IVertexAttributes
    // where TEdgeAttributes : IEdgeAttributes;

/// <summary>
/// Basic vertex that is able to remember reference to one other vertex.
/// 
/// This can be used in path finding mechanisms for remembering vertex from which was this vertex discovered.
/// </summary>
/// <typeparam name="TEdge">Type of hold edges originating in this vertex.</typeparam>
/// <typeparam name="TVertexAttributes">Type of borne vertex attributes.</typeparam>
public interface IPredecessorRememberingVertex : IVertex
{
    IPredecessorRememberingVertex? Predecessor { get; set; }
}

public interface IEdgesContainingPredecessorRememberingVertex<TEdge> :
    IEdgesContainingVertex<TEdge>, IPredecessorRememberingVertex
    where TEdge : IEdge;

using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex.
/// 
/// This reference can be used for remembering predecessor vertex during path finding.
/// This interface should not be implemented. Its successor <see cref="IPredecessorRemembering{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface IRemembersPredecessors<out TVertex, out TEdge, TVertexAttributes, TEdgeAttributes> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes

{
    public TOut AcceptGeneric<TOut, TOtherParams>(IRemembersPredecessorsGenericVisitor<TOut, TVertexAttributes, TEdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams);
}

public interface IRemembersPredecessorsGenericVisitor<TOut, TVertexAttributes, TEdgeAttributes, TOtherParams>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
{
    public TOut GenericVisit<TPredecessorRemembering, TVertex, TEdge>( TPredecessorRemembering predecessorRememberingGraph, TOtherParams otherParams)
        where TPredecessorRemembering : IPredecessorRemembering<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
        where TVertex : class, IPredecessorRememberingVertex<TVertexAttributes>, IBasicVertex<TEdge, TVertexAttributes>
        where TEdge : IBasicEdge<TVertex, TEdgeAttributes>;
}
    
    
/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex.
/// 
/// This reference can be used for remembering predecessor vertex during path finding.
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph which have ability to remember one other vertex.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
public interface IPredecessorRemembering<out TVertex, out TEdge, TVertexAttributes, TEdgeAttributes> : 
    IRemembersPredecessors<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>, 
    ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
    where TVertex : class, IPredecessorRememberingVertex<TVertexAttributes>, IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{ }


// public interface IBasicPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> : 
    // IBasicVertex<IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes>, 
    // IPredecessorRememberingVertex
    // where TVertexAttributes : IVertexAttributes
    // where TEdgeAttributes : IEdgeAttributes
// {}

// public interface IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes> :
    // IBasicEdge<IBasicPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>
    // where TVertexAttributes : IVertexAttributes
    // where TEdgeAttributes : IEdgeAttributes
// {}
    
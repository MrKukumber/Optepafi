using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex.
/// 
/// This reference can be used for remembering predecessor vertex during path finding.
/// This interface should not be implemented. Its successor <see cref="IPredecessorRemembering{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface IRemembersPredecessors<out TVertex, out TEdge> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
{}
    
    
/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex.
/// 
/// This reference can be used for remembering predecessor vertex during path finding.
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph which have ability to remember one other vertex.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
public interface IPredecessorRemembering<TVertexAttributes, TEdgeAttributes> : 
    IRemembersPredecessors<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>>, 
    ISearchable<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{ }


public interface IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> : 
    IBasicVertex<IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes>, 
    IPredecessorRememberingVertex
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{}

public interface IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes> :
    IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{}
    
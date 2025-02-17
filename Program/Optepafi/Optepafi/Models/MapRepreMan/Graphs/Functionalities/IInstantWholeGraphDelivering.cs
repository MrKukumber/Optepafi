using Optepafi.Models.MapRepreMan.VerticesAndEdges;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents graph type that is able to deliver whole graph (all its vertices) at once.
/// 
/// That means this map representations that corresponds to graphs with this ability must generate their graphs before start of path finding on them.
/// This interface should not be implemented. Its successor <see cref="IInstantWholeGraphDelivering{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface ICanDeliverWholeGraphInInstant<out TVertex, out TEdge> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge{}

/// <summary>
/// Represents graph type that is able to deliver whole graph (all its vertices) at once.
/// 
/// That means this map representations that corresponds to graphs with this ability must generate their graphs before start of path finding on them. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices of graph.</typeparam>
public interface IInstantWholeGraphDelivering<out TVertex, out TEdge> : ICanDeliverWholeGraphInInstant<TVertex, TEdge>, IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
{
    /// <summary>
    /// Returns array of all vertices of generated graph.
    /// </summary>
    /// <returns>All vertices of generated graph.</returns>
    TVertex[] GetWholeGraph();
}
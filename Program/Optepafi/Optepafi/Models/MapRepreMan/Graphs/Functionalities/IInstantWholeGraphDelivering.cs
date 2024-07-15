using Optepafi.Models.MapRepreMan.VertecesAndEdges;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents graph type that is able to deliver whole graph (all its vertices) at once.
/// 
/// That means this map representations that corresponds to graphs with this ability must generate their graphs before start of path finding on them. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices of graph.</typeparam>
public interface IInstantWholeGraphDelivering<out TVertex>
    where TVertex : IVertex
{
    /// <summary>
    /// Returns array of all vertices of generated graph.
    /// </summary>
    /// <returns>All vertices of generated graph.</returns>
    TVertex[] GetWholeGraph();
}
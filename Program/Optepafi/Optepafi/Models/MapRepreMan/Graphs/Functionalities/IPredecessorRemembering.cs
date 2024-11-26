using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex.
/// 
/// This reference can be used for remembering predecessor vertex during path finding.
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph which have ability to remember one other vertex.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
public interface IPredecessorRemembering<out TVertex, out TEdge> : ISearchable<TVertex, TEdge>
    where TVertex : IPredecessorRememberingVertex, IEdgesContainingVertex<TEdge>
    where TEdge : IDestVertexContainingEdge<TVertex>
{
    /// <summary>
    /// Explicit implementation of method GetVertexFor of ISearchable interface for better convenience of using this interface.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    
    // TVertex ISearchable<TVertex, TEdge>.GetVertexFor(MapCoordinates coords)
    // {
        // return GetVertexFor(coords);
    // }
    
    /// <summary>
    /// Returns vertex of graph which corresponds to provided location <see cref="MapCoordinates"/> and has ability to remember one other vertex.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    // new TVertex GetVertexFor(MapCoordinates coords);
}

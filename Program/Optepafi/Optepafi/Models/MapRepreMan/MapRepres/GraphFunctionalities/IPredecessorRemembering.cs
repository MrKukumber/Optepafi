using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;

/// <summary>
/// Represents searchable graph whose vertices has ability to remember one other vertex. This reference can be used for remembering predecessor vertex during path finding.
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph which have ability to remember one other vertex.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
public interface IPredecessorRemembering<out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes> :
    ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
    where TVertex : IPredecessorRememberingVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
    TVertex ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>.GetVertexFor(MapCoordinate coords)
    {
        return GetVertexFor(coords);
    }
    
    /// <summary>
    /// Returns vertex of graph which corresponds to provided location <see cref="MapCoordinate"/> and has ability to remember one other vertex.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    new TVertex GetVertexFor(MapCoordinate coords);
}

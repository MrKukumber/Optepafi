using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;


/// <summary>
/// Represents searchable graph. 
/// This interface should not be implemented. Its successor <see cref="ISearchable{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface ICanBeSearched<out TVertex, out TEdge> : IGraph<TVertex, TEdge> 
    where TVertex : IVertex
    where TEdge : IEdge
{ }


/// <summary>
/// Represents searchable graph. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
public interface ISearchable<out TVertex, out TEdge> : ICanBeSearched<TVertex, TEdge>, IGraph<TVertex, TEdge>
    where TVertex : IEdgesContainingVertex<TEdge>
    where TEdge : IDestVertexContainingEdge<TVertex>
{
    /// <summary>
    /// Returns vertex of graph which corresponds to provided location <see cref="MapCoordinates"/>.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    TVertex GetVertexFor(MapCoordinates coords);
}


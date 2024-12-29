using System.Runtime.InteropServices;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;


/// <summary>
/// Represents searchable graph. 
/// This interface should not be implemented. Its successor <see cref="ISearchable{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface ICanBeSearched<out TVertex, out TEdge, TVertexAttributes, TEdgeAttributes> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public TOut AcceptGeneric<TOut, TOtherParams>(ICanBeSearchedGenericVisitor<TOut, TVertexAttributes, TEdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams);
}

public interface ICanBeSearchedGenericVisitor<TOut, TVertexAttributes, TEdgeAttributes, TOtherParams>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public TOut GenericVisit<TGraph, TVertex, TEdge>(TGraph graph, TOtherParams otherParams)
        where TGraph : ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
        where TVertex : class, IBasicVertex<TEdge, TVertexAttributes>
        where TEdge : IBasicEdge<TVertex, TEdgeAttributes>;
}


/// <summary>
/// Represents searchable graph. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
public interface ISearchable<out TVertex, out TEdge, TVertexAttributes, TEdgeAttributes> : ICanBeSearched<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>, IGraph<TVertex, TEdge>
    where TVertex : class, IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Returns vertex of graph which corresponds to provided location <see cref="MapCoordinates"/>.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    TVertex GetVertexFor(MapCoordinates coords);
}


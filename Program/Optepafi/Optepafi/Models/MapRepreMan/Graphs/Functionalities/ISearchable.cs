using System.Runtime.InteropServices;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;


/// <summary>
/// Represents searchable graph. 
/// This interface should not be implemented. Its successor <see cref="ISearchable{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface ICanBeSearched<out TVertex, out TEdge, TWeight, TVertexAttributes, TEdgeAttributes> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public TOut AcceptGeneric<TOut, TOtherParams>(ICanBeSearchedGenericVisitor<TOut, TWeight, TVertexAttributes, TEdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams);
}

public interface ICanBeSearchedGenericVisitor<TOut, TWeight, TVertexAttributes, TEdgeAttributes, TOtherParams>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public TOut GenericVisit<TGraph, TVertex, TEdge>(TGraph graph, TOtherParams otherParams)
        where TGraph : ISearchable<TVertex, TEdge, TWeight, TVertexAttributes, TEdgeAttributes>
        where TVertex : class, IBasicVertex<TEdge, TVertexAttributes>
        where TEdge : IBasicEdge<TVertex, TEdgeAttributes, TWeight>;
}


/// <summary>
/// Represents searchable graph. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
/// <typeparam name="TWeight">Type, that is used for representing weight of the edge.</typeparam>
public interface ISearchable<out TVertex, out TEdge, TWeight, TVertexAttributes, TEdgeAttributes> : ICanBeSearched<TVertex, TEdge, TWeight, TVertexAttributes, TEdgeAttributes>, IGraph<TVertex, TEdge>
    where TVertex : class, IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes, TWeight>
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


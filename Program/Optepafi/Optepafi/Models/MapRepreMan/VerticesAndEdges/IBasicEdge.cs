using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VerticesAndEdges;

/// <summary>
/// Edge that represents basic edge functionality: it can bear attributes of defined type and can hold reference of its destination vertex.
/// </summary>
/// <typeparam name="TVertex">Type of hold destination vertex.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of borne edge attributes.</typeparam>
/// <typeparam name="TWeight">Type, that is used for representing weight of the edge.</typeparam>
public interface IBasicEdge<out TVertex, out TEdgeAttributes, TWeight> :
    IWeightedEdge<TWeight>,
    IDestVertexContainingEdge<TVertex>,
    IAttributesBearingEdge<TEdgeAttributes>
    where TEdgeAttributes : IEdgeAttributes
    where TVertex : IVertex;

/// <summary>
/// Oriented edge that can hold reference of its destination vertex.
/// </summary>
/// <typeparam name="TVertex">Type of hold destination vertex.</typeparam>
public interface IDestVertexContainingEdge<out TVertex> : IEdge
    where TVertex : IVertex
{
    /// <summary>
    /// Returns destination vertex of this edge.
    /// </summary>
    TVertex To { get; }
}

/// <summary>
/// Oriented edge that can bear some attributes of defined type.
/// </summary>
/// <typeparam name="TEdgeAttributes">Type of borne edge attributes.</typeparam>
public interface IAttributesBearingEdge<out TEdgeAttributes> : IEdge
    where TEdgeAttributes : IEdgeAttributes
{
    TEdgeAttributes Attributes { get; }
}

/// <summary>
/// Oriented, weighted edge of some graph.
/// </summary>
/// <typeparam name="TWeight">Type, that is used for representing weight of the edge.</typeparam>
public interface IWeightedEdge<TWeight> : IEdge
{
    /// <summary>
    /// Sets weight of this edge.
    /// </summary>
    /// <param name="weight">Weight to be set.</param>
    void SetWeight(TWeight weight);
    /// <summary>
    /// Method weight of this edge.
    /// </summary>
    /// <returns>Value of the weight of the edge. <c>float.NaN</c>, if weight was not set for this edge yet.</returns>
    TWeight GetWeight();
}

/// <summary>
/// Base interface representing oriented edge of some graph.
/// </summary>
public interface IEdge
{
    
}




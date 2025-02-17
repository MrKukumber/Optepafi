using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VerticesAndEdges;

/// <summary>
/// Edge that represents basic edge functionality: it can bear attributes of defined type and can hold reference of its destination vertex.
/// </summary>
/// <typeparam name="TVertex">Type of hold destination vertex.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of borne edge attributes.</typeparam>
public interface IBasicEdge<out TVertex, out TEdgeAttributes> :
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
/// Base interface representing oriented edge of some graph.
/// </summary>
public interface IEdge
{
    
}




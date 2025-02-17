using System.Collections.Generic;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VerticesAndEdges;

/// <summary>
/// Basic vertex whose type of hold edges is assign to be of type <see cref="IBasicEdge{TVertex,TEdgeAttributes}"/>.
/// 
/// This is just auxiliary, quality of life improving interface. 
/// </summary>
/// <typeparam name="TVertexAttributes">Type of borne vertex attributes.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes borne by assigned type of hold edges.</typeparam>
public interface IBasicEdgeCoupledBasicVertex<TVertexAttributes, TEdgeAttributes> :
    IBasicVertex<IBasicEdge<IBasicEdgeCoupledBasicVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>,
        TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes;

/// <summary>
/// Vertex that presents basic vertex functionality: it can bear attributes of defined type and can hold outgoing edges of defined type.
/// </summary>
/// <typeparam name="TEdge">Type of hold edges originating in this vertex.</typeparam>
/// <typeparam name="TVertexAttributes">Type of borne vertex attributes.</typeparam>
public interface IBasicVertex<TEdge, out TVertexAttributes> :
    IEdgesContainingVertex<TEdge>,
    IAttributesBearingVertex<TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdge : IEdge;

/// <summary>
/// Vertex that is able to return references to oriented edges of defined type originating in this vertex.
///
/// It also provides methods for setting and getting weights of these edges.
/// </summary>
/// <typeparam name="TEdge">Type of hold edges originating in this vertex.</typeparam>
public interface IEdgesContainingVertex<TEdge> : IVertex
    where TEdge : IEdge
{
    /// <summary>
    /// Method for getting collection of edges originated in this vertex.
    /// </summary>
    /// <returns>Collection of edges originated in vertex.</returns>
    IEnumerable<TEdge> GetEdges();
    
    /// <summary>
    /// Sets weight of provided edge. If edge is not present in vertex, it sets nothing.
    /// </summary>
    /// <param name="weight">Weight to be set for edge.</param>
    /// <param name="edge">Edge the weight is going to be set for.</param>
    void SetWeight(float weight, TEdge edge);
    
    /// <summary>
    /// Method for getting the weight of some edge. If edges wight is not computed yet or it is not present in vertex, returns null.
    /// </summary>
    /// <param name="edge">Edge which weight is proposed.</param>
    /// <returns>Weight of given edge. Float.Nan, if given edges weight is not computed yet or it is not present in vertex.</returns>
    bool TryGetWeight(TEdge edge, out float weight);
}

/// <summary>
/// Vertex that can bear some attributes of defined type.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of borne vertex attributes.</typeparam>
public interface IAttributesBearingVertex<out TVertexAttributes> : IVertex
    where TVertexAttributes : IVertexAttributes
{
    TVertexAttributes Attributes { get; }
}

/// <summary>
/// Base interface representing vertex of some graph.
/// </summary>
public interface IVertex
{
    
}
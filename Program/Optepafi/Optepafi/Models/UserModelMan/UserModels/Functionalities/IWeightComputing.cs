using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.UserModelMan.UserModels.Functionalities;

/// <summary>
/// Represents ability to compute weights from provided edge and vertex attributes.
///
/// User models which implement this interface can be used in path finding algorithms for computing weights of graph edges.  
/// IMPORTANT!! Weights are integer value of type int32. This standard holds for whole application, every implementation.  
/// </summary>
/// <typeparam name="TTemplate">Associated template type.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which is user model able to use for computing weights.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which is user model able to use for computing weights.</typeparam>
public interface IWeightComputing<out TTemplate, in TVertexAttributes, in TEdgeAttributes> : IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Computes weights form provided vertex and edge attributes.
    /// </summary>
    /// <param name="from">Vertex attributes from start position vertex.</param>
    /// <param name="through">Edge attributes of edge, for which is weight computed.</param>
    /// <param name="to">Vertex attributes from destination vertex.</param>
    /// <returns>Weight of edge.</returns>
    int ComputeWeight(TVertexAttributes from, TEdgeAttributes through, TVertexAttributes to);
}
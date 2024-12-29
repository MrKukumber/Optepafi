using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.UserModelMan.UserModels.Functionalities;

/// <summary>
/// Represents user model that is able to compute A* heuristics for provided vertex attributes.
///
/// This ability is specific need of A* searching algorithm.
/// </summary>
/// <typeparam name="TTemplate">Associated template type.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which is user model able to use for computing of the heuristics.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes associated with <c>TTemplate</c>.</typeparam>
public interface
    IAStarHeuristicsComputing<out TTemplate, in TVertexAttributes, in TEdgeAttributes> : IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Returns weight of heuristics between two vertices represented by their attributes.
    /// </summary>
    /// <param name="from">Origin vertex attributes of computed heuristics.</param>
    /// <param name="to">Destination vertex attributes of computed heuristics.</param>
    /// <returns>Value of heuristics.</returns>
    float GetWeightFromHeuristics(TVertexAttributes from, TVertexAttributes to);
}
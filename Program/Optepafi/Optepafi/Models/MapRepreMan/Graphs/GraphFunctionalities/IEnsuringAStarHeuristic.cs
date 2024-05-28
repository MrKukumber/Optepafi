using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.GraphFunctionalities;

/// <summary>
/// Represents graph type that is able to compute A* heuristics for provided vertex couple. This ability is specific need of A* searching algorithms.
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes provided by vertices of graph.</typeparam>
public interface IAStarHeuristicEnsuring<in TVertex, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TVertex : IAttributeBearingVertex<TVertexAttributes>
{
    /// <summary>
    /// Returns weight of Heuristics between two vertices.
    /// </summary>
    /// <param name="from">Origin vertex of computed heuristics.</param>
    /// <param name="to">Destination vertex of computed heuristics.</param>
    /// <returns>Value of heuristics.</returns>
    int GetWeightFromHeuristic(TVertex from, TVertex to);
}
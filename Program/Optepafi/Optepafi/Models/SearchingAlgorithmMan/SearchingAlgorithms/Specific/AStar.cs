using Optepafi.Models.SearchingAlgorithmMan.Configurations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations.AStar;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms.Specific;

/// <summary>
/// This class represents classic A* algorithm, which is combination of Djikstra's algorithm used to find the shortest path
/// and Greedy Best-First-Search algorithm used as guiding heuristics.
/// </summary>
public class AStar : SearchingAlgorithm<AStarConfiguration>
{
    public static AStar Instance = new AStar(); 
    private AStar() { }
    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}"/> 
    public override string Name { get; } = "A*";
    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}"/> 
    public override ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; } = [AStarGeneral.Instance];
    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}"/> 
    public override AStarConfiguration DefaultConfiguration { get; } = new ();
}
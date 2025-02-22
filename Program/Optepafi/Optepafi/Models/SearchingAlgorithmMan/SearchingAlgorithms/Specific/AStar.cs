using Optepafi.Models.SearchingAlgorithmMan.Configurations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations.AStar;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms.Specific;

/// <summary>
/// This class represents classic A* algorithm, which is 
/// </summary>
public class AStar : SearchingAlgorithm<AStarConfiguration>
{
    public static AStar Instance = new AStar(); 
    private AStar() { }
    public override string Name { get; } = "A*";
    public override ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; } = [AStarGeneral.Instance];
    public override AStarConfiguration DefaultConfiguration { get; } = new ();
}
using Optepafi.Models.SearchingAlgorithmMan.Implementations;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;

public class SmileyFacesDrawer : ISearchingAlgorithm
{
    public static SmileyFacesDrawer Instance { get; } = new();
    private SmileyFacesDrawer(){}
    
    public string Name { get; } = "Smiley face drawing";
    public ISearchingAlgoritmImplementation[] Implementations { get; } = [SmileyFacesDrawerGeneral.Instance];
}
using Optepafi.Models.SearchingAlgorithmMan.Implementations;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;

public class SmileyFaceDrawing : ISearchingAlgorithm
{
    public static SmileyFaceDrawing Instance { get; } = new();
    private SmileyFaceDrawing(){}
    
    public string Name { get; } = "Smiley face drawing";
    public ISearchingAlgoritmImplementation[] Implementations { get; } = [SmileyFaceDrawingGeneral.Instance];
}
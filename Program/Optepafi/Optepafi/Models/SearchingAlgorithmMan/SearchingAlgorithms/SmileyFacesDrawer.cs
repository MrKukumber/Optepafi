using Optepafi.Models.SearchingAlgorithmMan.Implementations;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;

/// <summary>
/// Algorithm that simply draws smiley faces in areas defined by individual legs.
/// This type is just demonstrative algorithm for presenting application functionality.
/// For more information on searching algorithms see <see cref="ISearchingAlgorithm"/>.
/// </summary>
public class SmileyFacesDrawer : ISearchingAlgorithm
{
    public static SmileyFacesDrawer Instance { get; } = new();
    private SmileyFacesDrawer(){}
    
    /// <inheritdoc cref="ISearchingAlgorithm.Name"/>
    public string Name { get; } = "Smiley face drawing";
    
    /// <inheritdoc cref="ISearchingAlgorithm.Implementations"/>
    public ISearchingAlgoritmImplementation[] Implementations { get; } = [SmileyFacesDrawerGeneral.Instance];
}
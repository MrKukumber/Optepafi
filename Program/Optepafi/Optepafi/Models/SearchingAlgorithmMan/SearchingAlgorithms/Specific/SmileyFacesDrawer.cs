using Optepafi.Models.SearchingAlgorithmMan.Implementations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations.SmileyFaceDrawer;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms.Specific;

/// <summary>
/// Algorithm that simply draws smiley faces in areas defined by individual legs.
/// 
/// This type is just demonstrative algorithm for presenting application functionality.  
/// For more information on searching algorithms see <see cref="ISearchingAlgorithm{TConfiguration}"/>.  
/// </summary>
public class SmileyFacesDrawer : ISearchingAlgorithm<NullConfiguration>
{
    public static SmileyFacesDrawer Instance { get; } = new();
    private SmileyFacesDrawer(){}

    /// <inheritdoc cref="ISearchingAlgorithm{TConfiguration}.DefaultConfiguration"/>
    public NullConfiguration DefaultConfiguration { get; } = new();

    /// <inheritdoc cref="ISearchingAlgorithm{TConfiguration}.Name"/>
    public string Name { get; } = "Smiley face drawer";
    
    /// <inheritdoc cref="ISearchingAlgorithm{TConfiguration}.Implementations"/>
    public ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; } = [SmileyFacesDrawerGeneral.Instance];
}
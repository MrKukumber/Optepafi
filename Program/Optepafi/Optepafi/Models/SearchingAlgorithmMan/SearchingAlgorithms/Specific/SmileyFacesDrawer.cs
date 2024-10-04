using Optepafi.Models.SearchingAlgorithmMan.Implementations;
using Optepafi.Models.SearchingAlgorithmMan.Implementations.SmileyFaceDrawer;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms.Specific;

/// <summary>
/// Algorithm that simply draws smiley faces in areas defined by individual legs.
/// 
/// This type is just demonstrative algorithm for presenting application functionality.  
/// For more information on searching algorithms see <see cref="SearchingAlgorithm{TConfiguration}"/>.  
/// </summary>
public class SmileyFacesDrawer : SearchingAlgorithm<NullConfiguration>
{
    public static SmileyFacesDrawer Instance { get; } = new();
    private SmileyFacesDrawer(){}

    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}.DefaultConfiguration"/>
    public override NullConfiguration DefaultConfiguration { get; } = new();

    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}.Name"/>
    public override string Name { get; } = "Smiley face drawer";
    
    /// <inheritdoc cref="SearchingAlgorithm{TConfiguration}.Implementations"/>
    public override ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; } = [SmileyFacesDrawerGeneral.Instance];
}
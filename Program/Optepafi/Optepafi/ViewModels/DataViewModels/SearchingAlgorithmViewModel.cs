using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.ViewModels.DataViewModels;

public class SearchingAlgorithmViewModel : DataViewModel<ISearchingAlgorithm>
{
    protected override ISearchingAlgorithm Data => SearchingAlgorithm;
    public ISearchingAlgorithm SearchingAlgorithm { get; }
    public SearchingAlgorithmViewModel(ISearchingAlgorithm searchingAlgorithm)
    {
        SearchingAlgorithm = searchingAlgorithm;
    }

    public string Name => SearchingAlgorithm.Name;
}
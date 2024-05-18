using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.ViewModels.DataViewModels;

public class SearchingAlgorithmViewModel : ViewModelBase
{
    public ISearchingAlgorithm SearchingAlgorithm { get; }
    public SearchingAlgorithmViewModel(ISearchingAlgorithm searchingAlgorithm)
    {
        SearchingAlgorithm = searchingAlgorithm;
    }

    public string Name => SearchingAlgorithm.Name;
}
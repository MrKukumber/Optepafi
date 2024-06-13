using Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Representatives;

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
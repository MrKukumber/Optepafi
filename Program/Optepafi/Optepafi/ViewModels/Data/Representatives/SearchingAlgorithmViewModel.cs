using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.ViewModels.Data.Representatives;


/// <summary>
/// Wrapping ViewModel for <c>ISearchingAlgorithm</c> type.
/// 
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
/// <param name="searchingAlgorithm">Searching algorithm instance to which will be this ViewModel coupled.</param>
public class SearchingAlgorithmViewModel(ISearchingAlgorithm searchingAlgorithm) : WrappingDataViewModel<ISearchingAlgorithm>
{
    
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override ISearchingAlgorithm Data => SearchingAlgorithm;
    /// <summary>
    /// Coupled searching algorithm instance.
    /// </summary>
    public ISearchingAlgorithm SearchingAlgorithm { get; } = searchingAlgorithm;
    public string Name => SearchingAlgorithm.Name;
    public IConfiguration DefaultConfigurationCopy => SearchingAlgorithm.DefaultConfigurationDeepCopy;
}
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Main;

/// <summary>
/// ModelView which is responsible for logic behind elevation data management and their acquisition.
/// This ModelView is special by its design. It is singleton class. Reason for this is its ability to be used simultaneously from multiple ViewModels.
/// Its work load includes:
/// - credential and not credential based elevation data downloading for requested regions
/// - removal of elevation data corresponding to requested region
/// - keeping hold of ViewModels for all Regions of all elevation data distributions. Thees region ViewModels are provided to corresponding ViewModel so it could use them for showing to user. The main idea behind them is that they are always the same instances. They can therefore hold their information and provide it across multiple elevation configurations at once.
/// 
/// For more information on ModelViews see <see cref="ModelViewBase"/>.
/// </summary>
public sealed class ElevDataModelView : ModelViewBase
{
    
    public static ElevDataModelView Instance { get; } = new();

    private ElevDataModelView()
    {
        TopRegionsOfAllDistributions = ElevDataManager.Instance.ElevDataSources
            .SelectMany(elevDataSource => elevDataSource.ElevDataDistributions)
            .ToDictionary(
                elevDataDistr => new ElevDataDistributionViewModel(elevDataDistr), 
                elevDataDistr => elevDataDistr.AllTopRegions
                    .Select(topRegion => new TopRegionViewModel(topRegion)).ToList());
    }
    
    /// <summary>
    /// Dictionary, which holds all top regions for all elevation data distributions in application.
    /// It is used by view model for accessing of region view models, which it can then use for showing them to user.
    /// This dictionary is key component of this model views ability to be used concurrently at multiple elevation data configurations at the same time.
    /// It ensures, that all elevation data configurations will hve the same information about presence of regions.
    /// </summary>
    public Dictionary<ElevDataDistributionViewModel, List<TopRegionViewModel>> TopRegionsOfAllDistributions { get; }
    
    /// <summary>
    /// Method for retrieving of all elevation data sources that are available in <c>ElevDataManager</c>. 
    /// </summary>
    /// <returns>All retrieved elevation data sources.</returns>
    public IEnumerable<ElevDataSourceViewModel> GetElevDataSources() => ElevDataManager.Instance.ElevDataSources.Select(elevDataSource => new ElevDataSourceViewModel(elevDataSource));

    /// <summary>
    /// Method for asynchronous elevation data download for requested region from provided elevation data distribution without requiring credentials for download.
    /// <c>ElevDataManager</c>s appropriate method is called.
    /// </summary>
    /// <param name="elevDataDistributionViewModel">Elevation data distribution from which elevation data of requested region should be downloaded.</param>
    /// <param name="regionViewModel">Region which corresponding elevation data should be downloaded.</param>
    /// <returns>Task with result of data download.</returns>
    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsNotRequiringElevDataDistributionViewModel elevDataDistributionViewModel,
        RegionViewModel regionViewModel)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataDistribution, region,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }

    
    /// <summary>
    /// Method for asynchronous elevation data download for requested region from provided elevation data distribution with requiring credentials for download.
    /// <c>ElevDataManager</c>s appropriate method is called.
    /// </summary>
    /// <param name="elevDataDistributionViewModel">Elevation data distribution from which elevation data of requested region should be downloaded.</param>
    /// <param name="regionViewModel">Region which corresponding elevation data should be downloaded.</param>
    /// <param name="credential">Credentials for downloading of elevation data.</param>
    /// <returns>Task with result of data download.</returns>
    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsRequiringElevDataDistributionViewModel elevDataDistributionViewModel,
        RegionViewModel regionViewModel, NetworkCredential credential)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataDistribution, region, credential,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }
    
    /// <summary>
    /// Method for removal of elevation data of requested region which were downloaded form specified elevation data distribution.
    /// <c>ElevDataManager</c>s appropriate method is called.
    /// </summary>
    /// <param name="elevDataDistributionViewModel">Elevation data distribution which downloaded data should be removed.</param>
    /// <param name="regionViewModel">Region which corresponding elevation data should be removed.</param>
    public async Task RemoveAsync(ElevDataDistributionViewModel elevDataDistributionViewModel, RegionViewModel regionViewModel)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        await Task.Run(() => ElevDataManager.Instance.RemoveRegion(elevDataDistribution, region));
    }
    
}
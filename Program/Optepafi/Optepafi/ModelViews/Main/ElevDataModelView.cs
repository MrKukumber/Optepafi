using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Main;

public sealed class ElevDataModelView : ModelViewBase
{
    private ElevDataModelView(){}
    public static ElevDataModelView Instance { get; } = new();

    public IEnumerable<ElevDataSourceViewModel> ElevDataSoruceViewModels { get; } = ElevDataManager.Instance.ElevDataSources.Select(elevDataSource => new ElevDataSourceViewModel(elevDataSource));

    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsNotRequiringElevDataDistributionViewModel elevDataDistributionViewModel,
        RegionViewModel regionViewModel)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataDistribution, region,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }

    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsRequiringElevDataDistributionViewModel elevDataDistributionViewModel,
        RegionViewModel regionViewModel, NetworkCredential credential)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataDistribution, region, credential,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }
    public async Task RemoveAsync(ElevDataDistributionViewModel elevDataDistributionViewModel, RegionViewModel regionViewModel)
    {
        var elevDataDistribution = elevDataDistributionViewModel.ElevDataDistribution;
        var region = regionViewModel.Region;
        await Task.Run(() => ElevDataManager.Instance.RemoveRegion(elevDataDistribution, region));
    }
}
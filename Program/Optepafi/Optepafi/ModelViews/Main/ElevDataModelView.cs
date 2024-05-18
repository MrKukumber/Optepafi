using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Main;

public sealed class ElevDataModelView : ModelViewBase
{
    private ElevDataModelView(){}
    public static ElevDataModelView Instance { get; } = new();

    public IEnumerable<ElevDataSourceViewModel> ElevDataSoruceViewModels { get; } = ElevDataManager.Instance.ElevDataSources.Select(elevDataSource => new ElevDataSourceViewModel(elevDataSource));

    public ElevDataTypeViewModel? GetElevDataType(string typeName)
    {
        foreach (var elevDataSourceViewModel in ElevDataSoruceViewModels)
        {
            foreach (var elevDataTypeViewModel in elevDataSourceViewModel.ElevDataTypes)
            {
                if (elevDataTypeViewModel.ElevDataType.GetType().Name == typeName)
                {
                    return elevDataTypeViewModel;
                }
            }
        }
        return null;
    }

    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsNotRequiringElevDataTypeViewModel elevDataTypeViewModel,
        RegionViewModel regionViewModel)
    {
        var elevDataType = elevDataTypeViewModel.ElevDataType;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataType, region,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }

    public async Task<ElevDataManager.DownloadingResult> DownloadAsync(CredentialsRequiringElevDataTypeViewModel elevDataTypeViewModel,
        RegionViewModel regionViewModel, NetworkCredential credential)
    {
        var elevDataType = elevDataTypeViewModel.ElevDataType;
        var region = regionViewModel.Region;
        return await Task.Run(() => ElevDataManager.Instance.DownloadRegion(elevDataType, region, credential,
            regionViewModel.DownloadingCancellationTokenSource.Token));
    }
    public async Task RemoveAsync(ElevDataTypeViewModel elevDataTypeViewModel, RegionViewModel regionViewModel)
    {
        var elevDataType = elevDataTypeViewModel.ElevDataType;
        var region = regionViewModel.Region;
        await Task.Run(() => ElevDataManager.Instance.RemoveRegion(elevDataType, region));
    }
}
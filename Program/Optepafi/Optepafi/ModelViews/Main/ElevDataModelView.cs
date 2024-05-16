using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ViewModels.Main.DataViewModels;

namespace Optepafi.ModelViews.Main;

public sealed class ElevDataModelView : ModelViewBase
{
    private ElevDataModelView(){}
    public static ElevDataModelView Instance { get; } = new();

    public IEnumerable<ElevDataSourceViewModel> ElevDataSources { get; } = ElevDataManager.Instance.ElevDataSources.Select(elevDataSource => new ElevDataSourceViewModel(elevDataSource));

    public ElevDataTypeViewModel? GetElevDataType(string typeName)
    {
        foreach (var elevDataSource in ElevDataSources)
        {
            foreach (var elevDataType in elevDataSource.ElevDataTypes)
            {
                if (elevDataType.GetType().Name == typeName)
                {
                    return elevDataType;
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
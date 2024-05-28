using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ViewModels.DataViewModels;

public class ElevDataSourceViewModel : DataViewModel<IElevDataSource>
{
    protected override IElevDataSource Data => ElevDataSource;
    public IElevDataSource ElevDataSource { get; }
    public ElevDataSourceViewModel(IElevDataSource elevDataSource)
    {
        ElevDataSource = elevDataSource;
    }

    public string Name => ElevDataSource.Name;

    public IEnumerable<ElevDataDistributionViewModel> ElevDataDistributions => ElevDataSource.ElevDataDistributions
        .SelectMany<IElevDataDistribution, ElevDataDistributionViewModel>(elevDataType => elevDataType switch
        {
            ICredentialsNotRequiringElevDataDistribution cnredt => [new CredentialsNotRequiringElevDataDistributionViewModel(cnredt)],
            ICredentialsRequiringElevDataDistribution credt => [new CredentialsRequiringElevDataDistributionViewModel(credt)],
            _ => []
        });
}
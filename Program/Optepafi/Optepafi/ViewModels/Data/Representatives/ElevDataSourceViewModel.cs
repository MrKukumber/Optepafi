using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.ElevSources;

namespace Optepafi.ViewModels.Data.Representatives;


/// <summary>
/// Wrapping ViewModel for <c>IElevDataSource</c> type.
/// 
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.
/// </summary>
/// <param name="elevDataSource">Elevation data source instance to which will be this ViewModel coupled.</param>
public class ElevDataSourceViewModel(IElevDataSource elevDataSource) : WrappingDataViewModel<IElevDataSource>
{
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override IElevDataSource Data => ElevDataSource;

    /// <summary>
    /// Coupled elevation data source instance.
    /// </summary>
    public IElevDataSource ElevDataSource { get; } = elevDataSource;

    public string Name => ElevDataSource.Name;

    /// <summary>
    /// Collection of ViewModels of elevation data distributions contained in elevation data source.
    /// 
    /// Distributions are divided to credentials-requiring and non credential-requiring ones.
    /// </summary>
    public IEnumerable<ElevDataDistributionViewModel> ElevDataDistributions => ElevDataSource.ElevDataDistributions
        .SelectMany<IElevDataDistribution, ElevDataDistributionViewModel>(elevDataDistr => elevDataDistr switch
        {
            ICredentialsNotRequiringElevDataDistribution cnredt => [new CredentialsNotRequiringElevDataDistributionViewModel(cnredt)],
            ICredentialsRequiringElevDataDistribution credt => [new CredentialsRequiringElevDataDistributionViewModel(credt)],
            _ => []
        });
}
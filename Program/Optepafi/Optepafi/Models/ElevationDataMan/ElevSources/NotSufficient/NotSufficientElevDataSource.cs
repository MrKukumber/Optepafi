using System.Collections.Generic;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.Specific.NotSufficient;

namespace Optepafi.Models.ElevationDataMan.ElevSources.NotSufficient;

/// <summary>
/// Source of elevation data, which never contains elevation data for provided map.
/// 
/// This type is just demonstrative data source for presenting application functionality.  
/// For more information on elevation data sources see <see cref="IElevDataSource"/>.  
/// </summary>
public class NotSufficientElevDataSource : IElevDataSource
{
    
    public static NotSufficientElevDataSource Instance { get; } = new();
    private NotSufficientElevDataSource(){}

    /// <inheritdoc cref="IElevDataSource.Name"/>
    public string Name => "Not sufficient elevation data source";
    /// <inheritdoc cref="IElevDataSource.ElevDataDistributions"/>
    public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; } = new HashSet<IElevDataDistribution>{ NotSufficientElevDataDistribution.Instance };
}
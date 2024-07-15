using System.Collections.Generic;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.Simulating;

namespace Optepafi.Models.ElevationDataMan.ElevSources.Simulating;

/// <summary>
/// Elevation data source that simulates work of real data source.
/// 
/// This type is just demonstrative elevation data source for presenting application functionality.  
/// For more information on elevation data sources see <see cref="IElevDataSource"/>.  
/// </summary>
public class SimulatingElevDataSource : IElevDataSource
{
    public static SimulatingElevDataSource Instance { get; } = new();
    private SimulatingElevDataSource(){}

    /// <inheritdoc cref="IElevDataSource.Name"/>
    public string Name => "Simulates elevation data source";
    
    /// <inheritdoc cref="IElevDataSource.ElevDataDistributions"/>
    public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; } = new HashSet<IElevDataDistribution>{ AuthorizationSimulatingElevDataDistribution.Instance, NoAuthorizationSimulatingElevDataDistribution.Instance};
}
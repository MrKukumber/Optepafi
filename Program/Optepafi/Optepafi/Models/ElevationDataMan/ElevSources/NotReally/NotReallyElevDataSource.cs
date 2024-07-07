using System.Collections.Generic;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.NotReally;

namespace Optepafi.Models.ElevationDataMan.ElevSources.NotReally;

public class NotReallyElevDataSource : IElevDataSource
{
    public static NotReallyElevDataSource Instance { get; } = new();
    private NotReallyElevDataSource(){}

    public string Name => "Not really an elevation data source";
    public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; } = new HashSet<IElevDataDistribution>{ NotReallyElevDataDistribution.Instance};
}
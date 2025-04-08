using System.Collections.Generic;
using System.ComponentModel;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;

// using Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;

namespace Optepafi.Models.ElevationDataMan.ElevSources;

public class USGS : IElevDataSource
{
    public static USGS Instance { get; } = new ();
    private USGS() { }
    public string Name { get; } = "USGS";
    // public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; } 
    public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; } = new HashSet<IElevDataDistribution>{ UsgsSrtm1ArcSecondGlobal.Instance };
}
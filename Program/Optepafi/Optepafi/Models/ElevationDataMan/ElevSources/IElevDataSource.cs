using System.Collections.Generic;
using Optepafi.Models.ElevationDataMan.Distributions;

namespace Optepafi.Models.ElevationDataMan.ElevSources;

/// <summary>
/// Represents source of elevation data. This source holds set of various data distributions, that it can provide.
/// For more information on elevation data distributions see <see cref="IElevDataDistribution"/>
/// </summary>
public interface IElevDataSource
{
    public string Name { get; }
    public IReadOnlySet<IElevDataDistribution> ElevDataDistributions { get; }
}
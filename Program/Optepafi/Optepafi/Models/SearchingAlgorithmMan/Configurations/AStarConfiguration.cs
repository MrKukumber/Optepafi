using System.Collections.Immutable;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.SearchingAlgorithmMan.Configurations;

/// <summary>
/// Configuration used to adjust the behaviour of A* algorithm.
/// At this moment, no configurations for A* are created.
/// </summary>
public class AStarConfiguration : IConfiguration
{
    /// <inheritdoc cref="IConfiguration"/> 
    public ImmutableList<IConfigItem> ConfigItems { get; } = ImmutableList<IConfigItem>.Empty;
    /// <inheritdoc cref="IConfiguration"/> 
    public IConfiguration DeepCopy() => new AStarConfiguration();
}
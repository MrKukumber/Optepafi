using System.Collections.Immutable;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.SearchingAlgorithmMan.Configurations;

//TODO: comment
public class AStarConfiguration : IConfiguration
{
    public ImmutableList<IConfigItem> ConfigItems { get; } = ImmutableList<IConfigItem>.Empty;
    public IConfiguration DeepCopy()
    {
        return new AStarConfiguration();
    }
}
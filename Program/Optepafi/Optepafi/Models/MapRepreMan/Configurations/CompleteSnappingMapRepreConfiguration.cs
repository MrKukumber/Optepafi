using System.Collections.Immutable;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Configurations;

//TODO: comment + add configs if necessary
public class CompleteSnappingMapRepreConfiguration : IConfiguration
{
    public ImmutableList<IConfigItem> ConfigItems { get; } = [];
    public IConfiguration DeepCopy()
    {
        return new CompleteSnappingMapRepreConfiguration();
    }
}
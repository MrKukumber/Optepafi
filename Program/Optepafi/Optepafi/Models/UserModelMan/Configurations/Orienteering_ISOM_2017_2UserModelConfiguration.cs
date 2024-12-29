using System.Collections.Immutable;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.UserModelMan.Configurations;

//TODO: comment
public class Orienteering_ISOM_2017_2UserModelConfiguration : IConfiguration
{
    public ImmutableList<IConfigItem> ConfigItems { get; } = ImmutableList<IConfigItem>.Empty;
    public IConfiguration DeepCopy()
    {
        return new Orienteering_ISOM_2017_2UserModelConfiguration();
    }
}
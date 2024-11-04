using System.Collections.Immutable;

namespace Optepafi.Models.Utils.Configurations;


/// <summary>
/// Represents predecessor for every class that is used for representing configurations of some application process.
///
/// It contains declaration of collection of configuration items. Each of these items represents some configuration value of associated process.
/// </summary>
public interface IConfiguration
{
    ImmutableList<IConfigItem> ConfigItems { get; }

    IConfiguration DeepCopy();
}

/// <summary>
/// Basic implementation of <c>IConfiguration</c> interface, which should be used in those cases, when no configuration is used in specific process but selection of some configuration type is required.
/// </summary>
public sealed class NullConfiguration : IConfiguration
{
    public ImmutableList<IConfigItem> ConfigItems { get; } = ImmutableList<IConfigItem>.Empty;
    public IConfiguration DeepCopy() => new NullConfiguration();
}

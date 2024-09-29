using System.Collections.Generic;

namespace Optepafi.Models.Utils;

public interface IConfiguration
{
    List<ConfigItem> ConfigItems { get; }
}

public class NullConfiguration : IConfiguration
{
    public List<ConfigItem> ConfigItems { get; } = new();
}
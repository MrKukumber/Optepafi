using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.MapMan.MapFormats;

public sealed class OMAPFormat : IMapFormat<OMAP>
{
    static public OMAPFormat Instance { get; } = new OMAPFormat();
    private OMAPFormat(){}
}
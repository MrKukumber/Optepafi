using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public sealed class OMAP : Map
{
    public IMapFormat<OMAP> MapFormat { get; } = OMAPFormat.Instance;
}
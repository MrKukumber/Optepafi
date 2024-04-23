using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public sealed class OMAP : IMap
{
    public IMapFormat<IMap> MapFormat { get; init; } 
}
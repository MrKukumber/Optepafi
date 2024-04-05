using System.Collections.Generic;
using System.IO;
using Optepafi.Models.MapRepre;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public static class MapManager
{
    public static IMapFormat[] MapFormats { get; } = { /*TODO: doplnit existujuce mapove formaty */ };

    public static IMap GetMapOfFrom(IMapFormat mapFormat, StreamReader file)
    {
        return mapFormat.CreateMap(file);
    }
}
using System.Collections.Generic;
using System.IO;
using Optepafi.Models.MapRepreMan;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public static class MapManager
{
    public static IMapFormat<Map>[] MapFormats { get; } = { /*TODO: doplnit existujuce mapove formaty */ };

    public static Map GetMapOfFrom(IMapFormat<Map> mapFormat, StreamReader file)
    {
        return mapFormat.CreateMap(file);
    }
}
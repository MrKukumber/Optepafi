using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Optepafi.Models.MapRepreMan;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public static class MapManager
{
    public static HashSet<IMapFormat<IMap>> MapFormats { get; } = new HashSet<IMapFormat<IMap>>();

    public static IMap GetMapOfFrom(IMapFormat<IMap> mapFormat, StreamReader file)
    {
        return mapFormat.CreateMapFrom(file);
    }
}
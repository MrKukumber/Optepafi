using System;
using System.IO;

namespace Optepafi.Models.MapMan;

public interface IMapFormat<TMap> where TMap : Map
{
    string Extension { get; }
    string MapFormatName { get; }
    TMap? CastMap(Map map)
    {
        if (map is TMap cMap) return cMap;
        return null;
    }
    TMap CreateMap(StreamReader inputFile);
}
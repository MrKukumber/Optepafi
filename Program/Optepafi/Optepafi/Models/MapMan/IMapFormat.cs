using System;
using System.IO;

namespace Optepafi.Models.MapMan;

public interface IMapFormat<out TMap> where TMap : IMap
{
    string Extension { get; }
    string MapFormatName { get; }
    MapManager.MapCreationResult CreateMapFrom(StreamReader inputMapFile, out IMap? map);
}

using System;
using System.IO;

namespace Optepafi.Models.MapMan;

public interface IMapFormat<out TMap> where TMap : IMap
{
    string Extension { get; }
    string MapFormatName { get; }
    TMap? CreateMapFrom(StreamReader inputMapFile, out MapManager.MapCreationResult creationResult);

}

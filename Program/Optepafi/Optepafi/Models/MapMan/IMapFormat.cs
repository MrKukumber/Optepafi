using System;
using System.IO;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

public interface IMapFormat<out TMap> where TMap : IMap
{
    string Extension { get; }
    string MapFormatName { get; }
    TMap? CreateMapFrom(StreamReader inputMapFile, out MapManager.MapCreationResult creationResult);

}

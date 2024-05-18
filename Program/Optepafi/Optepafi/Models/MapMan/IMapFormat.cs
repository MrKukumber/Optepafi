using System;
using System.IO;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

public interface IMapFormat<out TMap> where TMap : IMap
{
    string Extension { get; }
    string MapFormatName { get; }
    TMap? CreateMapFrom(Stream inputMapStream, CancellationToken? cancellationToken, out MapManager.MapCreationResult creationResult);

}

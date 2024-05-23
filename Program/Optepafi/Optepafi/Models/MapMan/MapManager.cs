using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public class MapManager : IMapGenericVisitor<IMapFormat<IMap>>
{
    public static MapManager Instance { get; } = new();
    private MapManager(){}
    public ISet<IMapFormat<IMap>> MapFormats { get; } = ImmutableHashSet.Create<IMapFormat<IMap>>();

    public IMapFormat<IMap> GetFormat(IMap map)
    {
        return map.AcceptGeneric(this);
    }
    IMapFormat<IMap> IMapGenericVisitor<IMapFormat<IMap>>.GenericVisit<TMap>(TMap map) 
    {
        foreach (var mapFormat in MapFormats)
        {
            if (mapFormat is IMapIdentifier<TMap>)
                return mapFormat;
        }
        throw new ArgumentException("Given map was not created by any existing map format.");
    }

    public IMapFormat<IMap>? GetCorrespondingMapFormatTo(string mapFileName)
    {
        foreach (var mapFormat in MapFormats)
        {
            if (Regex.IsMatch(mapFileName, ".*" + mapFormat.Extension + "$") ) return mapFormat;
        }
        return null;
    }
    public enum MapCreationResult{Ok, Incomplete, FileNotFound, UnableToParse, Cancelled}
    public MapCreationResult GetMapFromOf((Stream,string) mapStreamWithPath, IMapFormat<IMap> mapFormat, CancellationToken? cancellationToken, out IMap? map)
    {
        map = mapFormat.CreateMapFrom(mapStreamWithPath, cancellationToken, out MapCreationResult creationResult);
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return MapCreationResult.Cancelled;
        return creationResult;
    }
}
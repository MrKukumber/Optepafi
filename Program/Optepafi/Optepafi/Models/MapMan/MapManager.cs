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
public class MapManager
{
    public static MapManager Instance { get; } = new();
    private MapManager(){}
    public ISet<IMapFormat<IMap>> MapFormats { get; } = new HashSet<IMapFormat<IMap>>(); // TODO: premysliet, jak to reprezentaovat
    public IMapFormat<IMap>? GetCorrespondingMapFormatTo(string mapFileName)
    {
        foreach (var mapFormat in MapFormats)
        {
            if (Regex.IsMatch(mapFileName, ".*" + mapFormat.Extension + "$") ) return mapFormat;
        }
        return null;
    }
    public enum MapCreationResult{Ok, Incomplete, FileNotFound, UnableToParse, Cancelled}
    public MapCreationResult GetMapFromOf(Stream mapStream, IMapFormat<IMap> mapFormat, CancellationToken? cancellationToken, out IMap? map)
    {
        map = mapFormat.CreateMapFrom(mapStream, cancellationToken, out MapCreationResult creationResult);
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return MapCreationResult.Cancelled;
        return creationResult;
    }
    // public MapCreationResult GetMapFromOf(string fileName, IMapFormat<IMap> mapFormat, CancellationToken? cancellationToken, out IMap? map)
    // {
        // try
        // {
            // using (FileStream mapStream = new FileStream(fileName, FileMode.Open))
            // {
                // map = mapFormat.CreateMapFrom(mapStream, cancellationToken, out MapCreationResult creationResult);
                // return creationResult;
            // }
        // }
        // catch (System.IO.FileNotFoundException ex)
        // {
            // map = null;
            // return MapCreationResult.FileNotFound;
        // }
    // }
}
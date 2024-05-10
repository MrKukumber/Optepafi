using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public class MapManager
{
    public static MapManager Instance { get; } = new();
    private MapManager(){}

    private ISet<IMapFormat<IMap>> MapFormats { get; } = new HashSet<IMapFormat<IMap>>(); // TODO: premysliet, jak to reprezentaovat

    public enum MapCreationResult{Ok, Incomplete, UnableToOpen, UnableToParse }
    public MapCreationResult GetMapOfFrom(IMapFormat<IMap> mapFormat, StreamReader file, out IMap? map)
    {
        map = mapFormat.CreateMapFrom(file, out MapCreationResult creationResult);
        return creationResult;
    }
    public MapCreationResult GetMapOfFrom(IMapFormat<IMap> mapFormat, string fileName, out IMap? map)
    {
        try
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                map = mapFormat.CreateMapFrom(file, out MapCreationResult creationResult);
                return creationResult;
            }
        }
        catch (System.IO.IOException ex)
        {
            map = null;
            return MapCreationResult.UnableToOpen;
        }
    }
}
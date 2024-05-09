using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

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
        return mapFormat.CreateMapFrom(file, out map);
    }
    public MapCreationResult GetMapOfFrom(IMapFormat<IMap> mapFormat, string fileName, out IMap? map)
    {
        try
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                return mapFormat.CreateMapFrom(file, out map);
            }
        }
        catch (System.IO.IOException ex)
        {
            map = null;
            return MapCreationResult.UnableToOpen;
        }
    }
}
using System.IO;

namespace Optepafi.Models.MapMan;

public abstract class MapFormat<TMap> : IMapFormat<TMap> where TMap : IMap, new()
{
    public abstract string Extension { get; }
    public abstract string MapFormatName { get; }
    public MapManager.MapCreationResult CreateMapFrom(StreamReader inputMapFile, out IMap? map)
    {
        map = new TMap()
        {
            MapFormat = (IMapFormat<IMap>)this
        };
        return map.FillUp(inputMapFile);
    }
}
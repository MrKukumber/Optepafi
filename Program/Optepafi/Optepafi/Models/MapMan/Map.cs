namespace Optepafi.Models.MapMan;

public abstract class Map
{
    public IMapFormat<Map> MapFormat { get; }

    protected Map(IMapFormat<Map> mapFormat)
    {
        MapFormat = mapFormat;
    } 
}
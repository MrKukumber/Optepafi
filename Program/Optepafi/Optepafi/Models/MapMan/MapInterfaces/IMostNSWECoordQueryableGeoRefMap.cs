namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IMostNSWECoordQueryableGeoRefMap : IGeoReferencedMap
{
    public MapCoordinate MostNorthCoord { get; }
    public MapCoordinate MostSouthCoord { get; }
    public MapCoordinate MostWestCoord { get; }
    public MapCoordinate MostEastCoord { get; }
}
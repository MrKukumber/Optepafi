namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IMostNSWECoordQueryableGeoLocMap : IGeoLocatedMap
{
    public GeoCoordinate MostNorthCoord { get; }
    public GeoCoordinate MostSouthCoord { get; }
    public GeoCoordinate MostWestCoord { get; }
    public GeoCoordinate MostEastCoord { get; }
}
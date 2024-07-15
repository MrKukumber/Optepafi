namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="GeoCoordinate"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoLocMap : IGeoLocatedMap
{
    public GeoCoordinate NorthernmostCoord { get; }
    public GeoCoordinate SouthernmostCoord { get; }
    public GeoCoordinate WesternmostCoord { get; }
    public GeoCoordinate EasternmostCoord { get; }
}


/// <summary>
/// Represents geo-referenced maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="MapCoordinate"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoRefMap : IGeoReferencedMap
{
    public MapCoordinate NorthernmostCoords { get; }
    public MapCoordinate SouthernmostCoords { get; }
    public MapCoordinate WesternmostCoords { get; }
    public MapCoordinate EasternmostCoords { get; }
}

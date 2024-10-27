namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="GeoCoordinate"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoLocMap : IAreaQueryableMap
{
    public GeoCoordinate NorthernmostCoords { get; }
    public GeoCoordinate SouthernmostCoords { get; }
    public GeoCoordinate WesternmostCoords { get; }
    public GeoCoordinate EasternmostCoords { get; }
}


/// <summary>
/// Represents geo-referenced maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="MapCoordinate"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoRefMap : IAreaQueryableMap
{
    public MapCoordinate NorthernmostCoords { get; }
    public MapCoordinate SouthernmostCoords { get; }
    public MapCoordinate WesternmostCoords { get; }
    public MapCoordinate EasternmostCoords { get; }
}

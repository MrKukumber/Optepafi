using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="GeoCoordinates"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoLocMap : IAreaQueryableMap
{
    public GeoCoordinates NorthernmostCoords { get; }
    public GeoCoordinates SouthernmostCoords { get; }
    public GeoCoordinates WesternmostCoords { get; }
    public GeoCoordinates EasternmostCoords { get; }
}


/// <summary>
/// Represents geo-referenced maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="MapCoordinates"/>.
/// </summary>
public interface IMostNSWECoordQueryableGeoRefMap : IAreaQueryableMap
{
    public MapCoordinates NorthernmostCoords { get; }
    public MapCoordinates SouthernmostCoords { get; }
    public MapCoordinates WesternmostCoords { get; }
    public MapCoordinates EasternmostCoords { get; }
}

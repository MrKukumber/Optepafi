using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="GeoCoordinates"/>.
/// </summary>
public interface IBoxBoundedGeoLocMap : IAreaQueryableMap
{
    public GeoCoordinates SouthWestBoundingCorner { get; }
    public GeoCoordinates NorthEastBoundingCorner { get; }
}


/// <summary>
/// Represents geo-referenced maps ability to provide its northernmost, southernmost, westernmost and easternmost coordinate.
/// 
/// This locations are provided by <see cref="MapCoordinates"/>.
/// </summary>
public interface IBoxBoundedGeoRefMap : IAreaQueryableMap, IScaledMap
{
    public MapCoordinates BottomLeftBoundingCorner { get; }
    public MapCoordinates TopRightBoundingCorner { get; }
}

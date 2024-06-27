namespace Optepafi.Models.MapMan;

/// <summary>
/// Represents position on map relative to (hopefully geo-referenced) origin. This coordinate system is the one in which application logic communicates.
/// Values of coordinates are measured in maps micrometers. Point (1000, 0) is therefore in 1 millimeter distance from the origin on map.
/// If maps scale is 1:10000, point (1,0) is in the real world positioned in 1 cm distance from the origin of the map. In this manner we should be able to wrap whole Globe to the map of this scale or smaller.
/// This coordinate system is meant for maps that are not implicitly represented in geographic coordinate system.
/// Every map representation can use its own coordinate system for describing maps. However, it should contain conversion mechanism either to <c>MapCoordinate</c> system or to <see cref="GeoCoordinate"/> system, so it could effectively communicate with other parts of application.
/// </summary>
/// <param name="XPos">Position on horizontal axis.</param>
/// <param name="YPos">Position on vertical axis.</param>
public record struct MapCoordinate(int XPos, int YPos)
{
    public static MapCoordinate operator -(MapCoordinate coordinate1, MapCoordinate coordinate2)
    {
        return new MapCoordinate(coordinate1.XPos - coordinate2.XPos, coordinate1.YPos - coordinate2.YPos);
    }
} 

/// <summary>
/// Represents coordinate of geographic coordinate system (GCS).
/// Values of this struct represent longitude and latitude values of the coordinate.
/// This coordinate system is meant for maps that implicitly use GCS for representation of world features positions.
/// Every map representation can use its own coordinate system for describing maps. However, it should contain conversion mechanism either to <c>GeoCoordinate</c> system or to <see cref="MapCoordinate"/> system, so it could effectively communicate with other parts of application.
/// </summary>
/// <param name="Longitude">Longitude of coordinate.</param>
/// <param name="Latitude"> Latitude of coordinate.</param>
public record struct GeoCoordinate(double Longitude, double Latitude) { }

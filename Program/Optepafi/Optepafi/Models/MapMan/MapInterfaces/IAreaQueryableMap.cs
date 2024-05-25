namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to decide whether polygon provided by array of its vertex positions intersects with its own area.
/// In this case, coordinates of provided polygon are of <see cref="GeoCoordinate"/> type.
/// </summary>
public interface IDirectlyAreaQueryableMap : IGeoLocatedMap
{
    /// <summary>
    /// Method for resolution of intersection of map and provided polygon.
    /// </summary>
    /// <param name="polygonCoordinates">Array of polygons vertex locations.</param>
    /// <returns>True, if areas of map and polygon intersect.</returns>
    bool DoesIntersectWithPolygon(GeoCoordinate[] polygonCoordinates);
}

/// <summary>
/// Represents geo-referenced maps ability to decide whether polygon provided by array of its vertex positions intersects with its own area.
/// In this case, coordinates of provided polygon are of <see cref="MapCoordinate"/> type.
/// </summary>
public interface IByReferenceAreaQueryableMap : IGeoReferencedMap
{
    /// <summary>
    /// Method for resolution of intersection of map and provided polygon.
    /// </summary>
    /// <param name="polygonCoordinates">Array of polygons vertex locations.</param>
    /// <returns>True, if areas of map and polygon intersect. </returns>
    bool DoesIntersectWithPolygon(MapCoordinate[] polygonCoordinates);
}


using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents geo-located maps ability to decide whether polygon provided by array of its vertex positions intersects with its own area.
/// 
/// In this case, coordinates of provided polygon are of <see cref="GeoCoordinates"/> type.
/// </summary>
public interface IDirectlyAreaIntersectionQueryableMap : IAreaQueryableMap
{
    /// <summary>
    /// Method for resolution of intersection of map and provided polygon.
    /// </summary>
    /// <param name="polygonCoordinates">Array of polygons vertex locations.</param>
    /// <returns>True, if areas of map and polygon intersect.</returns>
    bool DoesIntersectWithPolygon(GeoCoordinates[] polygonCoordinates);
}

/// <summary>
/// Represents geo-referenced maps ability to decide whether polygon provided by array of its vertex positions intersects with its own area.
/// 
/// In this case, coordinates of provided polygon are of <see cref="MapCoordinates"/> type.
/// </summary>
public interface IByReferenceAreaIntersectionQueryableMap : IAreaQueryableMap
{
    /// <summary>
    /// Method for resolution of intersection of map and provided polygon.
    /// </summary>
    /// <param name="polygonCoordinates">Array of polygons vertex locations.</param>
    /// <returns>True, if areas of map and polygon intersect. </returns>
    bool DoesIntersectWithPolygon(MapCoordinates[] polygonCoordinates);
}


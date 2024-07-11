using Optepafi.Models.MapMan;

namespace Optepafi.Models.ElevationDataMan;

/// <summary>
/// Represents object, that can provide required elevation data. It is created by elevation data distribution for some map to measure.
/// For this purpose it defines two overloads of method <see cref="GetElevation(Optepafi.Models.MapMan.GeoCoordinate)"/> which supports <c>GeoCoordinate</c>s as well as <c>MapCoordinate</c>s with geo referenced origin.
/// Instance of <c>IElevData</c> is then used in map representation creation as supplementary source of data.
/// </summary>
public interface IElevData
{
    /// <summary>
    /// Method returning elevation of specified <see cref="GeoCoordinate"/>.
    /// </summary>
    /// <param name="coordinate">Coordinate for which elevation is returned.</param>
    /// <returns>Elevation.</returns>
    public double? GetElevation(GeoCoordinate coordinate);
    /// <summary>
    /// Method returning elevation of specified geo referenced <see cref="MapCoordinate"/>.
    /// </summary>
    /// <param name="coordinate">Coordinate for which elevation is returned.</param>
    /// <param name="geoReference">Geo reference of origin of map coordinate system.</param>
    /// <returns>Elevation.</returns>
    public double? GetElevation(MapCoordinate coordinate, GeoCoordinate geoReference);
}
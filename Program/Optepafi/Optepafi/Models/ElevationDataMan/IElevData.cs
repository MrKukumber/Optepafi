using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.Models.ElevationDataMan;

public interface IElevData
{
    public double? GetElevation(GeoCoordinate coordinate);
    public double? GetElevation(MapCoordinate coordinate, GeoCoordinate geoReference);
}
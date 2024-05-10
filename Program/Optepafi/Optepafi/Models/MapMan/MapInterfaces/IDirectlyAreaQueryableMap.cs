using Optepafi.Models.ElevationDataMan;

namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IDirectlyAreaQueryableMap : IGeoLocatedMap
{
    bool DoesIntersectWithPolynom(GeoCoordinate[] polynomCoordinates);
}


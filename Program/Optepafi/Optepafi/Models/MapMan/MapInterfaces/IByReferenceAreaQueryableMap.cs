using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IByReferenceAreaQueryableMap : IGeoReferencedMap
{
    bool DoesIntersectWithPolynom(MapCoordinate[] polynomCoordinates);
}
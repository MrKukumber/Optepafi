using Optepafi.Models.MapMan;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Represents one part of whole track. It is used in path finding for identifying the origin and desired destination of searched path.
/// </summary>
/// <param name="Start"><see cref="MapCoordinate"/> of the origin of a leg.</param>
/// <param name="Finish"><see cref="MapCoordinate"/> of the destination of a leg.</param>
public record struct Leg(MapCoordinate Start, MapCoordinate Finish);
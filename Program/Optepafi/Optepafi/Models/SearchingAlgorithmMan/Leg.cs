using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Represents one part of whole track.
///
/// It is used in path finding for identifying the origin and desired destination of searched path.
/// </summary>
/// <param name="Start"><see cref="MapCoordinates"/> of the origin of a leg.</param>
/// <param name="Finish"><see cref="MapCoordinates"/> of the destination of a leg.</param>
public record struct Leg(MapCoordinates Start, MapCoordinates Finish)
{
    
}
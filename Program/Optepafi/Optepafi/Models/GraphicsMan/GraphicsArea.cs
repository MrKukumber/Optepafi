using Optepafi.Models.MapMan;

namespace Optepafi.Models.GraphicsMan;

/// <summary>
/// Struct representing area of map, therefore are of its graphics. It defines rectangle that encapsulates whole map.
/// This rectangle is defined by its lef-bottom and top-right vertex coordinates.
/// </summary>
/// <param name="LeftBottomVertex">Left-bottom vertex of rectangle which defines maps area.</param>
/// <param name="TopRightVertex">Right-top vertex of rectangle which defines maps area. </param>
public record struct GraphicsArea(MapCoordinate LeftBottomVertex, MapCoordinate TopRightVertex);
using Optepafi.Models.MapMan;

namespace Optepafi.Models.GraphicsMan;

public record struct GraphicsArea(MapCoordinate LeftBottomVertex, MapCoordinate TopRightVertex) { }
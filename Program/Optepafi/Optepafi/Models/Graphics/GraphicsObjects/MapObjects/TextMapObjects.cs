using Optepafi.Models.MapMan;

namespace Optepafi.Models.Graphics.GraphicsObjects.MapObjects;

public record WordObject(MapCoordinate Position, string Text) : IGraphicObject
{ }

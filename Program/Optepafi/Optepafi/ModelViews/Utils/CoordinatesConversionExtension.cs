using Optepafi.Models.MapMan;
using Optepafi.ViewModels.Data;

namespace Optepafi.ModelViews.Utils;

/// <summary>
/// Static extension class that implements extension methods for converting between various types of coordinates.
/// </summary>
public static class CoordinatesConversionExtension
{
    /// <summary>
    /// Extension method for converting <c>CanvasCoordinate</c> to <c>MapCoordinate</c> according to provided maps left-bottom vertex position.
    /// Canvas coordinates are by default shifted so they correspond with axis of canvases. Therefore they have to be shifted back. 
    /// </summary>
    /// <param name="canvasCoordinate">Coordinates to be converted to map coordinates.</param>
    /// <param name="mapLeftBottomVertex">Coordinate of left-bottom vertex of corresponding GraphicsArea.</param>
    /// <returns>Converted coordinates.</returns>
    public static MapCoordinate ToMapCoordinate(this CanvasCoordinate canvasCoordinate,
        MapCoordinate mapLeftBottomVertex)
    {
        return new MapCoordinate(canvasCoordinate.LeftPos + mapLeftBottomVertex.XPos,
            canvasCoordinate.BottomPos + mapLeftBottomVertex.YPos);
    }

    /// <summary>
    /// Extension method for converting <c>MapCoordinate</c> to <c>CanvasCoordinate</c> according to provided maps left-bottom vertex position.
    /// Canvas coordinates are by default shifted so they correspond with axis of canvases. This shift is defined by the area of corresponding graphics, namely by its lef-bottom vertex.
    /// </summary>
    /// <param name="mapCoordinate">Coordinates to be converted to canvas coordinates.</param>
    /// <param name="mapLeftBottomVertex">Coordinate of left-bottom vertex of corresponding GraphicsArea.</param>
    /// <returns>Converted coordinates.</returns>
    public static CanvasCoordinate ToCanvasCoordinate(this MapCoordinate mapCoordinate,
        MapCoordinate mapLeftBottomVertex)
    {
        return new CanvasCoordinate(mapCoordinate.XPos - mapLeftBottomVertex.XPos,
            mapCoordinate.YPos - mapLeftBottomVertex.YPos);
    }
}
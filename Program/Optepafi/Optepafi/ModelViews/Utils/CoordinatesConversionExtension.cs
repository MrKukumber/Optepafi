using Avalonia.Controls;
using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data;

namespace Optepafi.ModelViews.Utils;

/// <summary>
/// Static extension class that implements extension methods for converting between various types of coordinates.
/// </summary>
public static class CoordinatesConversionExtension
{
    /// <summary>
    /// Extension method for converting <c>CanvasCoordinate</c> to <c>MapCoordinate</c> according to provided maps left-bottom vertex position.
    /// 
    /// Canvas coordinates are by default shifted so they correspond with axis of canvases. Therefore, they have to be shifted back.  
    /// </summary>
    /// <param name="canvasCoordinate">Coordinates to be converted to map coordinates.</param>
    /// <param name="mapTopLeftVertex">Coordinate of left-bottom vertex of corresponding GraphicsArea.</param>
    /// <returns>Converted coordinates.</returns>
    public static MapCoordinates ToMapCoordinate(this CanvasCoordinate canvasCoordinate,
        MapCoordinates mapTopLeftVertex)
    {
        return new MapCoordinates(canvasCoordinate.LeftPos + mapTopLeftVertex.XPos,
            mapTopLeftVertex.YPos - canvasCoordinate.TopPos);
    }

    /// <summary>
    /// Extension method for converting <c>MapCoordinate</c> to <c>CanvasCoordinate</c> according to provided maps left-bottom vertex position.
    /// 
    /// Canvas coordinates are by default shifted so they correspond with axis of canvases. This shift is defined by the area of corresponding graphics, namely by its lef-bottom vertex.  
    /// </summary>
    /// <param name="mapCoordinates">Coordinates to be converted to canvas coordinates.</param>
    /// <param name="mapTopLeftVertex">Coordinate of left-bottom vertex of corresponding GraphicsArea.</param>
    /// <returns>Converted coordinates.</returns>
    public static CanvasCoordinate ToCanvasCoordinate(this MapCoordinates mapCoordinates,
        MapCoordinates mapTopLeftVertex)
    {
        return new CanvasCoordinate(mapCoordinates.XPos - mapTopLeftVertex.XPos,
            mapTopLeftVertex.YPos - mapCoordinates.YPos);
    }

    //TODO: comment
    public static GeoCoordinates ToGeoCoordinate(this CanvasCoordinate canvasCoordinate,
        MapCoordinates mapTopLeftVertex, GeoCoordinates geoReference, int scale)
    {
        MapCoordinates mapCoords = canvasCoordinate.ToMapCoordinate(mapTopLeftVertex);
        return mapCoords.ToGeoCoordinates(geoReference, scale);
    }

    public static CanvasCoordinate ToCanvasCoordinate(this GeoCoordinates geoCoordinates,
        MapCoordinates mapTopLeftVertex, GeoCoordinates geoReference, int scale)
    {
        MapCoordinates mapCoords = geoCoordinates.ToMapCoordinates(geoReference, scale);
        return mapCoords.ToCanvasCoordinate(mapTopLeftVertex);
    }
}
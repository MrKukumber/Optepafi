namespace Optepafi.ViewModels.DataViewModels;

/// <summary>
/// Represents position on canvas relatively to its left-bottom corner. It is used mainly by ViewModels for communication with Views and ModelViews. Its up to ModelViews to convert these coordinates to map coordinates which are used in Model.
/// Values of coordinates are measured in micrometers of corresponding map. Point (1000, 0) is therefore in 1 millimeter distance from the left bottom corner of canvas.
/// If corresponding maps scale is 1:10000, point (1,0) is in the real world positioned in 1 cm distance from the bottom left vertex of canvas.
/// </summary>
/// <param name="LeftPos">Horizontal distance from canvases left-bottom vertex.</param>
/// <param name="BottomPos">Vertical distance from canvases left-bottom vertex.</param>
public record struct CanvasCoordinate(int LeftPos, int BottomPos)
{
    public static CanvasCoordinate operator -(CanvasCoordinate coordinate1, CanvasCoordinate coordinate2)
    {
        return new CanvasCoordinate(coordinate1.LeftPos - coordinate2.LeftPos, coordinate1.BottomPos - coordinate2.BottomPos);
    }
}
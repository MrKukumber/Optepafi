using System;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;

namespace Optepafi.ViewModels.Data;

/// <summary>
/// Represents position on canvas relatively to its left-bottom corner.
///
/// It is used mainly by ViewModels for communication with Views and ModelViews. It's up to ModelViews to convert these coordinates to map coordinates which are used in Model.  
/// Values of coordinates are measured in micrometers of corresponding map. Point (1000, 0) is therefore in 1 millimeter distance from the left top corner of canvas.  
/// If corresponding maps scale is 1:10000, point (1,0) is in the real world positioned in 1 cm distance from the top left vertex of canvas.  
/// </summary>
/// <param name="LeftPos">Horizontal distance from canvases bottom-left vertex.</param>
/// <param name="TopPos">Vertical distance from canvases top-right vertex.</param>
public record struct CanvasCoordinate(int LeftPos, int TopPos)
{
    public static CanvasCoordinate operator *(CanvasCoordinate coord, int num)
        => new CanvasCoordinate(coord.LeftPos * num, coord.TopPos * num);
    public static CanvasCoordinate operator *(int num, CanvasCoordinate coord)
        => coord * num;
    public static CanvasCoordinate operator *(CanvasCoordinate coord, double num)
        => new CanvasCoordinate((int)(coord.LeftPos * num), (int)(coord.TopPos * num));
    public static CanvasCoordinate operator *(double num, CanvasCoordinate coord)
        => coord * num;
    
    public static CanvasCoordinate operator -(CanvasCoordinate coordinate1, CanvasCoordinate coordinate2)
        => new CanvasCoordinate(coordinate1.LeftPos - coordinate2.LeftPos, coordinate1.TopPos - coordinate2.TopPos);
    public static CanvasCoordinate operator +(CanvasCoordinate coordinate1, CanvasCoordinate coordinate2)
        => new CanvasCoordinate(coordinate1.LeftPos + coordinate2.LeftPos, coordinate1.TopPos + coordinate2.TopPos);
    
    public CanvasCoordinate Rotate(float angle, CanvasCoordinate center)
    {
        int translatedLeft = LeftPos - center.LeftPos;
        int translatedTop = TopPos - center.TopPos;
        int rotatedTransposedLeft = (int)(translatedLeft * Math.Cos(angle) - translatedTop * Math.Sin(angle));
        int rotatedTransposedTop = (int)(translatedLeft * Math.Sin(angle) + translatedTop * Math.Cos(angle));
        return new CanvasCoordinate(rotatedTransposedLeft + center.LeftPos, rotatedTransposedTop + center.TopPos);
    }

    public CanvasCoordinate Rotate(float angle)
    {
        int rotatedTransposedLeft = (int)(LeftPos * Math.Cos(angle) - TopPos * Math.Sin(angle));
        int rotatedTransposedTop = (int)(LeftPos * Math.Sin(angle) + TopPos * Math.Cos(angle));
        return new CanvasCoordinate(rotatedTransposedLeft, rotatedTransposedTop);
    }

    public double Size() => Math.Sqrt(LeftPos * LeftPos + TopPos * TopPos);
}
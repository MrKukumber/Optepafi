namespace Optepafi.ViewModels.DataViewModels;

public record struct CanvasCoordinate(int LeftPos, int BottomPos)
{
    public static CanvasCoordinate operator -(CanvasCoordinate coordinate1, CanvasCoordinate coordinate2)
    {
        return new CanvasCoordinate(coordinate1.LeftPos - coordinate2.LeftPos, coordinate1.BottomPos - coordinate2.BottomPos);
    }
}
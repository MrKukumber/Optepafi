using Optepafi.Models.MapMan;
using Optepafi.ViewModels.DataViewModels;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Graphics.PathObjects;

public class SmileyFaceEyeObjectViewModel : GraphicObjectViewModel
{
    public SmileyFaceEyeObjectViewModel(CanvasCoordinate position, int width, int height)
    {
        Position = new CanvasCoordinate(position.LeftPos - width/2, position.BottomPos - height/2);
        Priority = 420;
        Width = width;
        Height = height;
    }
    public override CanvasCoordinate Position { get; }
    public override int Priority { get; }
    public int Width { get; }
    public int Height { get; }
}

public class SmileyFaceNoseObjectViewModel : GraphicObjectViewModel
{
    public SmileyFaceNoseObjectViewModel(CanvasCoordinate position, int width, int height)
    {
        Position = new CanvasCoordinate(position.LeftPos - width/2, position.BottomPos - height/2);
        Priority = 420;
        Width = width;
        Height = height;
    }
    public override CanvasCoordinate Position { get; }
    public override int Priority { get; }
    public int Width { get; }
    public int Height { get; }
}

public class SmileyFaceMouthObjectViewModel : GraphicObjectViewModel
{
    
    public SmileyFaceMouthObjectViewModel(CanvasCoordinate position, (MapCoordinate, MapCoordinate, MapCoordinate, MapCoordinate) data)
    {
        Position = position;
        Priority = 420;
        StartPoint = data.Item1;
        Point1 = data.Item2;
        Point2 = data.Item3;
        Point3 = data.Item4;
    }
    public override CanvasCoordinate Position { get; }
    public override int Priority { get; }
    public MapCoordinate StartPoint { get; }
    public MapCoordinate Point1 { get; }
    public MapCoordinate Point2 { get; }
    public MapCoordinate Point3 { get; }
}

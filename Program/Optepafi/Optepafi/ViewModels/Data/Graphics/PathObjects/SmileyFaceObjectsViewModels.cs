using Optepafi.Models.MapMan;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Graphics.PathObjects;

public class SmileyFaceEyeObjectViewModel : GraphicObjectViewModel
{
    public SmileyFaceEyeObjectViewModel(int leftPos, int bottomPos, int width, int height)
    {
        LeftPos = leftPos - width/2;
        BottomPos = bottomPos - height/2;
        Priority = 420;
        Width = width;
        Height = height;
    }
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    public int Width { get; }
    public int Height { get; }
}

public class SmileyFaceNoseObjectViewModel : GraphicObjectViewModel
{
    public SmileyFaceNoseObjectViewModel(int leftPos, int bottomPos, int width, int height)
    {
        LeftPos = leftPos - width/2;
        BottomPos = bottomPos - height/2;
        Priority = 420;
        Width = width;
        Height = height;
    }
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    public int Width { get; }
    public int Height { get; }
}

public class SmileyFaceMouthObjectViewModel : GraphicObjectViewModel
{
    
    public SmileyFaceMouthObjectViewModel(int leftPos, int bottomPos, (MapCoordinate, MapCoordinate, MapCoordinate, MapCoordinate) data)
    {
        LeftPos = leftPos;
        BottomPos = bottomPos;
        Priority = 420;
        StartPoint = data.Item1;
        Point1 = data.Item2;
        Point2 = data.Item3;
        Point3 = data.Item4;
    }
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    public MapCoordinate StartPoint { get; }
    public MapCoordinate Point1 { get; }
    public MapCoordinate Point2 { get; }
    public MapCoordinate Point3 { get; }
}

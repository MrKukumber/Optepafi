using Optepafi.Models.MapMan;

namespace Optepafi.ViewModels.Data.Graphics.PathObjects;

public class SmileyFaceObjectViewModel : GraphicObjectViewModel
{

    public SmileyFaceObjectViewModel(int leftPos, int bottomPos, (MapCoordinate, int ) eye1, (MapCoordinate, int) eye2,
        (MapCoordinate, int) nose, (MapCoordinate, MapCoordinate, MapCoordinate, MapCoordinate) mouth)
    {
        LeftPos = leftPos;
        BottomPos = bottomPos;
        Priority = 420;
        Eye1Pos = eye1.Item1;
        Eye1Rad = eye1.Item2;
        Eye2Pos = eye2.Item1;
        Eye2Rad = eye2.Item2;
        NosePos = nose.Item1;
        NoseRad = nose.Item2;
        MouthData = $"M {mouth.Item1.XPos},{mouth.Item1.YPos} c {mouth.Item2.XPos},{mouth.Item2.YPos} {mouth.Item3.XPos},{mouth.Item3.YPos} {mouth.Item4.XPos},{mouth.Item4.YPos} Z";
    }
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    
    public MapCoordinate Eye1Pos { get; }
    public int Eye1Rad { get; }
    public MapCoordinate Eye2Pos { get; }
    public int Eye2Rad { get; }
    public MapCoordinate NosePos { get; }
    public int NoseRad { get; }
    public string MouthData { get; }
}
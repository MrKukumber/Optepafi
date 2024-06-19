namespace Optepafi.ViewModels.Data.Graphics.MapObjects;


public class WordObjectViewModel : GraphicObjectViewModel
{
    public WordObjectViewModel(int leftPos, int bottomPos, string word)
    {
        LeftPos = leftPos;
        BottomPos = bottomPos;
        Priority = 0;
        Word = word;
    }
    
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    public string Word { get; }
}

public class TrackPointWordObjectViewModel : GraphicObjectViewModel
{
    public TrackPointWordObjectViewModel(int leftPos, int bottomPos)
    {
        LeftPos = leftPos - 1000;
        BottomPos = bottomPos - 2000;
        Priority = 1;
    }
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
}
namespace Optepafi.ViewModels.Data.Graphics.MapObjects;


public class WordObjectViewModel : GraphicObjectViewModel
{
    public WordObjectViewModel(string word, int leftPos, int bottomPos)
    {
        Word = word;
        LeftPos = leftPos;
        BottomPos = bottomPos;
        Priority = 0;
    }
    
    public override int LeftPos { get; }
    public override int BottomPos { get; }
    public override int Priority { get; }
    public string Word { get; }
}
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Graphics.MapObjects;


public class WordObjectViewModel : GraphicObjectViewModel
{
    public WordObjectViewModel(CanvasCoordinate position, string word)
    {
        Position = position;
        Priority = 0;
        Word = word;
    }
    
    public override CanvasCoordinate Position { get; }
    public override int Priority { get; }
    public string Word { get; }
}

public class TrackPointWordObjectViewModel : GraphicObjectViewModel
{
    public TrackPointWordObjectViewModel(CanvasCoordinate position)
    {
        Position = new CanvasCoordinate(position.LeftPos - 1000, position.BottomPos - 2000);
        Priority = 1;
    }
    public override CanvasCoordinate Position { get; }
    public override int Priority { get; }
}
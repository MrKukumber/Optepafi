using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Graphics;

public abstract class GraphicObjectViewModel : ReactiveObject
{
    
    // public abstract int LeftPos { get; }
    // public abstract int BottomPos { get; }
    public abstract CanvasCoordinate Position { get; }
    public abstract int Priority { get; }
}
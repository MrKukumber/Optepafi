using ReactiveUI;

namespace Optepafi.ViewModels.Graphics;

public abstract class GraphicObjectViewModel : ReactiveObject
{
    
    public abstract int LeftPos { get; }
    public abstract int BottomPos { get; }
    public abstract int Priority { get; }
}
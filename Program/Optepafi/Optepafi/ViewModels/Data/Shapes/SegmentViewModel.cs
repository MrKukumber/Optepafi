namespace Optepafi.ViewModels.Data.Shapes;

//TODO: comment
public abstract class SegmentViewModel : DataViewModel
{
    public abstract string GetStringRep();    
    public abstract CanvasCoordinate LastPoint { get; }
}
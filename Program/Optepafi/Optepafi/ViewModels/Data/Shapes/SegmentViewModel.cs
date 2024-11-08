namespace Optepafi.ViewModels.Data.Shapes;

//TODO: comment
public abstract class SegmentViewModel : DataViewModel
{
    public abstract string GetStringRep();    
    public abstract CanvasCoordinate LastPoint { get; }
    public abstract SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0);
    public abstract SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0, out CanvasCoordinate alignedPoint0);
}
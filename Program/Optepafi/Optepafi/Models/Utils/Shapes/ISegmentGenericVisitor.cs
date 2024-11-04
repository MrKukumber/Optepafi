namespace Optepafi.Models.Utils.Shapes;

//TODO: comment
public interface ISegmentVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit(CubicBezierCurveSegment segment, TOtherParams otherParams);
    public TOut GenericVisit(LineSegment segment, TOtherParams otherParams);
}
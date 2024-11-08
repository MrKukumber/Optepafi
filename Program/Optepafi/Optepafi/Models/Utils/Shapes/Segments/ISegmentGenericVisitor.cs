namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public interface ISegmentVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit(CubicBezierCurveSegment segment, TOtherParams otherParams);
    public TOut GenericVisit(QuadraticBezierCurveSegment segment, TOtherParams otherParams);
    public TOut GenericVisit(LineSegment segment, TOtherParams otherParams);
}
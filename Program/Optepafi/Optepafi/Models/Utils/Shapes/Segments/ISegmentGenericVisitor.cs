namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public interface ISegmentVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit(CubicBezierCurveSegment segment, TOtherParams otherParams);
    public TOut GenericVisit(QuadraticBezierCurveSegment segment, TOtherParams otherParams);
    public TOut GenericVisit(LineSegment segment, TOtherParams otherParams);
}
public interface ISegmentVisitor<TOtherParams>
{
    public void Visit(CubicBezierCurveSegment segment, TOtherParams otherParams);
    public void Visit(QuadraticBezierCurveSegment segment, TOtherParams otherParams);
    public void Visit(LineSegment segment, TOtherParams otherParams);
}

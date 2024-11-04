namespace Optepafi.Models.Utils.Shapes;

//TODO: comment
public record class CubicBezierCurveSegment(MapCoordinates Point1, MapCoordinates Point2, MapCoordinates Point3) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    public override MapCoordinates LastPoint => Point3;
}
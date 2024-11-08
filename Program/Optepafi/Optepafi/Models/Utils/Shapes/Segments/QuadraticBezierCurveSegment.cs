namespace Optepafi.Models.Utils.Shapes.Segments;

public record class QuadraticBezierCurveSegment(MapCoordinates Point1, MapCoordinates Point2) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override MapCoordinates LastPoint => Point2;
}
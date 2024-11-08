namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public record class CubicBezierCurveSegment(MapCoordinates Point1, MapCoordinates Point2, MapCoordinates Point3) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override MapCoordinates LastPoint => Point3;
}
using System;

namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public record class CubicBezierCurveSegment(MapCoordinates Point1, MapCoordinates Point2, MapCoordinates Point3) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override void Accept<TOtherParams>(ISegmentVisitor<TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.Visit(this, otherParams);
    public override MapCoordinates LastPoint => Point3;
    public override MapCoordinates PositionAt(double t, MapCoordinates point0)
    {
        if (t < 0 || t > 1) throw new ArgumentOutOfRangeException(nameof(t));
        return Math.Pow(1 - t, 3) * point0 + 3 * Math.Pow(1 - t, 2) * t * Point1 + 3 * (1 - t) * Math.Pow(t, 2) * Point2 + Math.Pow(t, 3) * Point3;
    }

    public override MapCoordinates d(double t, MapCoordinates point0) => 3 * Math.Pow(1 - t, 2) * (Point1 - point0)
                                                                         + 6 * (1 - t) * t * (Point2 - Point1)
                                                                         + 3 * t * t * (Point3 - Point2);
}
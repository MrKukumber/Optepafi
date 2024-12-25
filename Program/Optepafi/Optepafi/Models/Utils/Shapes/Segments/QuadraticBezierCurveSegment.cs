using System;

namespace Optepafi.Models.Utils.Shapes.Segments;

public record class QuadraticBezierCurveSegment(MapCoordinates Point1, MapCoordinates Point2) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override void Accept<TOtherParams>(ISegmentVisitor<TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.Visit(this, otherParams);
    public override MapCoordinates LastPoint => Point2;
    public override MapCoordinates PositionAt(double t, MapCoordinates point0)
    {
        if (t < 0 || t > 1) throw new ArgumentOutOfRangeException(nameof(t));
        return Math.Pow(1 - t, 2) * point0 + 2 * (1 - t) * t * Point1 + Math.Pow(t, 2) * Point2;
    }

    public override MapCoordinates d(double t, MapCoordinates point0) => 2 * (1 - t) * (Point1 - point0) 
                                                                         + 2 * t * (Point2 - Point1);
}
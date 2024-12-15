using System;

namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public record class LineSegment(MapCoordinates Point1) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override void Accept<TOtherParams>(ISegmentVisitor<TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.Visit(this, otherParams);
    public override MapCoordinates LastPoint => Point1;
    public override MapCoordinates PositionAt(double t, MapCoordinates point0)
    {
        if (t < 0 || t > 1) throw new ArgumentOutOfRangeException(nameof(t));
        return point0 + t * (Point1 - point0); 
    }
}
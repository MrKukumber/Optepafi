namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public record class LineSegment(MapCoordinates Point1) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams) => segmentVisitor.GenericVisit(this, otherParams);
    public override MapCoordinates LastPoint => Point1;
}
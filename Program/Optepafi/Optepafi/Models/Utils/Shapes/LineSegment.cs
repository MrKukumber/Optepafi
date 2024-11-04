namespace Optepafi.Models.Utils.Shapes;

//TODO: comment
public record class LineSegment(MapCoordinates Point1) : Segment
{
    public override TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    public override MapCoordinates LastPoint => Point1;
}
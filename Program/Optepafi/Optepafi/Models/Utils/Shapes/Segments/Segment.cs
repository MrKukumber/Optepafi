namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public abstract record class Segment
{
    public abstract TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams);
    public abstract void Accept<TOtherParams>(ISegmentVisitor<TOtherParams> segmentVisitor, TOtherParams otherParams);
    public abstract MapCoordinates LastPoint { get; }
    public abstract MapCoordinates PositionAt(double t, MapCoordinates point0);
}
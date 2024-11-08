namespace Optepafi.Models.Utils.Shapes.Segments;

//TODO: comment
public abstract record class Segment
{
    public abstract TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> segmentVisitor, TOtherParams otherParams);
    public abstract MapCoordinates LastPoint { get; }
}
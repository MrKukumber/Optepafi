namespace Optepafi.Models.Utils.Shapes;

//TODO: comment
public abstract record class Segment
{
    public abstract TOut Accept<TOut, TOtherParams>(ISegmentVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    public abstract MapCoordinates LastPoint { get; }
}
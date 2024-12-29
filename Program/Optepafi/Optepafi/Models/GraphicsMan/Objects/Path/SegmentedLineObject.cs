using Avalonia.Media;

namespace Optepafi.Models.GraphicsMan.Objects.Path;

//TODO: comment
public record class SegmentedLineObject(Utils.Shapes.Path Path) : IGraphicObject
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit(this, otherParams);
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
        => genericVisitor.GenericVisit(this);
}
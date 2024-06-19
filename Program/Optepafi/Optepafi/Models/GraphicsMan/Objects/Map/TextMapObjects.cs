using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.Graphics.GraphicsObjects.MapObjects;

public record WordObject(MapCoordinate Position, string Text) : IGraphicObject
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }

    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }
}

public record TrackPointWordObject(MapCoordinate Position) : IGraphicObject
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }

    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }
}

namespace Optepafi.Models.Graphics.Objects;

public interface IGraphicObject
{
    TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor);
}
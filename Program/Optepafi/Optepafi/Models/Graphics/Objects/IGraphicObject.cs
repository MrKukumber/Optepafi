namespace Optepafi.Models.Graphics.Objects;

public interface IGraphicObject
{
    TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor);
}
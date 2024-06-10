namespace Optepafi.Models.Graphics;

public interface IGraphicObjectGenericVisitor<TOut>
{
    TOut GenericVisit<TGraphicObject>(TGraphicObject graphicObject);
}
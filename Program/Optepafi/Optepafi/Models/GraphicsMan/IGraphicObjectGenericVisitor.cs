namespace Optepafi.Models.Graphics;


public interface IGraphicObjectGenericVisitor<TOut, TOtherParams>
{
    TOut GenericVisit<TGraphicObject>(TGraphicObject graphicObject, TOtherParams otherParams);
}

public interface IGraphicObjectGenericVisitor<TOut>
{
    TOut GenericVisit<TGraphicObject>(TGraphicObject graphicObject);
}
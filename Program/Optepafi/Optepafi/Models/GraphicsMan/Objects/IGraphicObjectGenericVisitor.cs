using Optepafi.Models.Graphics.Objects;

namespace Optepafi.Models.GraphicsMan.Objects;


/// <summary>
/// One of generic visitor interfaces for <see cref="IGraphicObject"/> implementations. It provides access to modified visitor pattern on graphic objects, where only one generic method is required to be implemented.
/// It serves mainly for acquiring generic parameter, that represents real type of visited object.
/// It has one more overload for convenience of use.
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of <c>GenericVisit</c>.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface IGraphicObjectGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="graphicObject">Graphic object which accepted the visit.</param>
    /// <returns></returns>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TGraphicObject">Type of accepting graphic object. Main result of this modified visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    TOut GenericVisit<TGraphicObject>(TGraphicObject graphicObject, TOtherParams otherParams) 
        where TGraphicObject : IGraphicObject;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="IGraphicObject"/> implementations. It provides access to modified visitor pattern on graphic objects.
/// For more information see <see cref="IGraphicObjectGenericVisitor{TOut,TOtherParams}"/> .
/// </summary>
public interface IGraphicObjectGenericVisitor<TOut>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information of this method see <see cref="IGraphicObjectGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    TOut GenericVisit<TGraphicObject>(TGraphicObject graphicObject) 
        where TGraphicObject : IGraphicObject;
}
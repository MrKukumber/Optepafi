using Optepafi.Models.GraphicsMan.Objects;

namespace Optepafi.Models.Graphics.Objects;

/// <summary>
/// This interface is used for definition of object, that is meant to be displayed for user on some canvas or image.
/// Each implementation of this interface represents some particular graphic object for which there should be defined some view model and template for rendering.
/// Graphic objects are aggregated in <see cref="GraphicsManager"/> / <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> by use of some aggregator. They are then collected, converted to view models and presented to user.
///
/// It provides modification of visitor pattern, so-called "generic visitor pattern".
/// The main goal is not ensuring that caller implements specific method for every <c>IGraphicObject</c> implementation, but for ability to retrieve objects real type in form of type parameter.
/// </summary>
public interface IGraphicObject
{
    
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>IGraphicObject</c> interface.
    /// For more information on this pattern see <see cref="IGraphicObjectGenericVisitor{TOut,TOtherParams}"/>
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor ;).</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of return value carried through visitor pattern.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of the rest of parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>IGraphicObject</c> interface.
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TOtherParams}"/>.
    /// </summary>
    TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor);
}
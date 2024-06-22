using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.Graphics.GraphicsObjects.MapObjects;

/// <summary>
/// Object representing one word which should be displayed for user. It contains its position and text itself.
/// </summary>
/// <param name="Position">Position of word.</param>
/// <param name="Text">Text of word.</param>
public record WordObject(MapCoordinate Position, string Text) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }
}

/// <summary>
/// Object that represents point of track in <see cref="TextMap"/>. The look should be always the same. It therefore defines only its position.
/// </summary>
/// <param name="Position">Position of point of the track.</param>
public record TrackPointWordObject(MapCoordinate Position) : IGraphicObject
{
    
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }
}

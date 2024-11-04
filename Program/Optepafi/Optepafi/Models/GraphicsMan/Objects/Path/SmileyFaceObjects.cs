using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan.Objects.Path;

/// <summary>
/// Object representing eye of smiley face. It defines its position, width and height.
/// </summary>
public class SmileyFaceEyeObject : IGraphicObject
{
    public SmileyFaceEyeObject(MapCoordinates position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    
    public MapCoordinates Position { get; }
    public int Width { get; }
    public int Height { get; }
    

}

/// <summary>
/// Object representing nose of smiley face. It defines its position, width and height.
/// </summary>
public class SmileyFaceNoseObject : IGraphicObject
{
    public SmileyFaceNoseObject(MapCoordinates position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    
    public MapCoordinates Position { get; }
    public int Width { get; }
    public int Height { get; }
    
}

/// <summary>
/// Object representing mouth of smiley face. It defines 4 coordinates of bezier curve by which mouth should be drawn.
/// </summary>
public class SmileyFaceMouthObject : IGraphicObject
{
    public SmileyFaceMouthObject(MapCoordinates pos1, MapCoordinates pos2, MapCoordinates pos3, MapCoordinates pos4)
    {
        BezierCurveData = (pos1, pos2, pos3, pos4);
    }
    
    
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    
    /// <summary>
    /// 4 coordinates of bezier curve by which mouth should be drawn.
    /// </summary>
    public (MapCoordinates pos1, MapCoordinates pos2, MapCoordinates pos3, MapCoordinates pos4) BezierCurveData;
}

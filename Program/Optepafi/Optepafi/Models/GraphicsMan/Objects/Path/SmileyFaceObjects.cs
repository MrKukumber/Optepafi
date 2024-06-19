using Optepafi.Models.MapMan;

namespace Optepafi.Models.Graphics.Objects.Path;

// public class SmileyFaceObject : IGraphicObject
// {
    // public SmileyFaceObject(MapCoordinate position)
    // {
        // Eye1 = (new MapCoordinate(position.XPos-5000, position.YPos+3000), 500);
        // Eye2 = (new MapCoordinate(position.XPos+5000, position.YPos+3000), 500);
        // Nose = (position, 500);
        // Mouth = (new MapCoordinate(position.XPos-5000, position.YPos-3000), new MapCoordinate(position.XPos-50000, position.YPos-7000), new MapCoordinate(position.XPos+50000, position.YPos-7000), new MapCoordinate(position.XPos+50000, position.YPos-3000));
    // }

    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    // {
        // return genericVisitor.GenericVisit(this, otherParams);
    // }

    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor)
    // {
        // return genericVisitor.GenericVisit(this);
    // }
    // public (MapCoordinate center, int radius) Eye1 { get; }
    // public (MapCoordinate center, int radius) Eye2 { get; }
    // public (MapCoordinate center, int radius) Nose { get; }
    // public (MapCoordinate , MapCoordinate, MapCoordinate, MapCoordinate) Mouth { get; }
// }

public class SmileyFaceEyeObject : IGraphicObject
{
    public SmileyFaceEyeObject(MapCoordinate position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    public MapCoordinate Position { get; }
    public int Width { get; }
    public int Height { get; }
    

}
public class SmileyFaceNoseObject : IGraphicObject
{
    public SmileyFaceNoseObject(MapCoordinate position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    public MapCoordinate Position { get; }
    public int Width { get; }
    public int Height { get; }
    
}
public class SmileyFaceMouthObject : IGraphicObject
{
    public SmileyFaceMouthObject(MapCoordinate origin, MapCoordinate pos2, MapCoordinate pos3, MapCoordinate pos4)
    {
        BezierCurveData = (origin, pos2, pos3, pos4);
    }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); }
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
    public (MapCoordinate, MapCoordinate, MapCoordinate, MapCoordinate) BezierCurveData;
}

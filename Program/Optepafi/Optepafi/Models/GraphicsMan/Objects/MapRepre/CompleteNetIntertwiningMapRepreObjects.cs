using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan.Objects.MapRepre;


public record VertexObject(MapCoordinates Position) : IGraphicObject
{
    
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

public record EdgeObject(MapCoordinates From, MapCoordinates To, Orienteering_ISOM_2017_2.EdgeAttributes Attributes) : IGraphicObject
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}
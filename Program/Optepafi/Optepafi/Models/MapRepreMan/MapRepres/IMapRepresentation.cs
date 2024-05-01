using System.Runtime.InteropServices.Marshalling;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IMapRepresentation
{
    public IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        IMapRepresentationGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepresentationGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IMapRepresentationGenericVisitor<TOut> genericVisitor);
    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor);
}

public interface IMapRepresentation<out TTemplate, out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes> :
    IMapRepresentation
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
    
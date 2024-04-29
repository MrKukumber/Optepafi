using System.Runtime.InteropServices.Marshalling;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IMapRepresentation<out TTemplate> where TTemplate : ITemplate
{
    public IMapRepreRepresentativ<IMapRepresentation<ITemplate>> MapRepreRep { get; init; }
    public TTemplate UsedTemplate { get; }

    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        IMapRepresentationGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepresentationGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IMapRepresentationGenericVisitor<TOut> genericVisitor);
    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor);
}
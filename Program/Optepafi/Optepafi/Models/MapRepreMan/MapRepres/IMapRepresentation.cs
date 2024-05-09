using Avalonia.Controls;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IMapRepresentation
{
    public IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; } //TODO: premysliet ci je to potrebne
    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        // IMapRepresentationGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    // public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepresentationGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams);
    // public TOut AcceptGeneric<TOut>(IMapRepresentationGenericVisitor<TOut> genericVisitor);
    // public void AcceptGeneric(ITemplateGenericVisitor genericVisitor);
}

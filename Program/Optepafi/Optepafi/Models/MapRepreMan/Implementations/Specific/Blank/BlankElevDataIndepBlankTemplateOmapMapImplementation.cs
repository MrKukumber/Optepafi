using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.Blank;

/// <summary>
/// Graph implementation of OMAP map representation which uses blank template and for its creation are not used any
/// elevation data.
/// This implementation of map representation is not usable because of blank template. It is just demonstrative type
/// for presenting application functionality.
/// </summary>
public class BlankElevDataIndepBlankTemplateOmapMapImplementation :
    IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    /// <inheritdoc cref="IGraph{TVertexAttributes,TEdgeAttributes}.RestoreConsistency"/>
    public void RestoreConsistency() { }

    /// <inheritdoc cref="IMapRepre.Name"/>
    public string Name { get; } = "Blank map representation.";

    /// <inheritdoc cref="IMapRepre.AcceptGeneric{TOut,TOtherParams}"/>
    TOut IMapRepre.AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    public TOut AcceptGeneric<TOut, TGenericParam1, TGenericParam2, TGenericConstraint1, TGenericConstraint2, TOtherParams>(
        IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams> genericVisitor, TOtherParams otherParams) where TGenericParam1 : TGenericConstraint1 where TGenericParam2 : TGenericConstraint2
        => genericVisitor.GenericVisit<BlankElevDataIndepBlankTemplateOmapMapImplementation , IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Vertex, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Edge, TGenericParam1, TGenericParam2>(this, otherParams);
}
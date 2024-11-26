using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.Blank;

//TODO: comment
public class BlankElevDataIndepBlankTemplateOmapMapImplementation :
    IGraph<IBlankGraph.Vertex<BlankTemplate.VertexAttributes>, IBlankGraph.Edge<BlankTemplate.EdgeAttributes>>,
    IBlankGraph
{
    /// <inheritdoc cref="IGraph{TVertexAttributes,TEdgeAttributes}.RestoreConsistency"/>
    public void RestoreConsistency() { }

    /// <inheritdoc cref="IMapRepre.Name"/>
    public string Name { get; } = "Blank map representation.";

    /// <inheritdoc cref="IMapRepre.AcceptGeneric{TOut,TOtherParams}"/>
    TOut IMapRepre.AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
}
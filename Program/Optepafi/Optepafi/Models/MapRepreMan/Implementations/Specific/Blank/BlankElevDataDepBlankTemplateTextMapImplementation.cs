using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.Blank;

/// <summary>
/// Implementation of blank map representation/graph for <c>BlankTemplate</c> template type and <c>TextMap</c> map type.
/// 
/// This implementation requires elevation data for its creation but does not use them.  
/// For more information on graphs see <see cref="IGraph{TVertexAttributes,TEdgeAttributes}"/> and on map representations <see cref="IMapRepre"/>.  
/// </summary>
public abstract class BlankElevDataDepBlankTemplateTextMapImplementation :
    IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    /// <inheritdoc cref="IGraph{TVertexAttributes,TEdgeAttributes}.RestoreConsistency"/>
    public void RestoreConsistency() { }

    /// <inheritdoc cref="IMapRepre.Name"/>
    public string Name { get; } = "Blank map representation.";
    
    /// <inheritdoc cref="IMapRepre.AcceptGeneric{TOut,TOtherParams}"/>
    TOut IMapRepre.AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit<BlankElevDataDepBlankTemplateTextMapImplementation, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>(this, otherParams);
}
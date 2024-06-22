using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific;

/// <summary>
/// Implementation of blank map representation/graph for <c>BlankTemplate</c> template type and <c>TextMap</c> map type. This implementation does not need elevation data for its creation.
/// For more information on graphs see <see cref="IGraph{TVertexAttributes,TEdgeAttributes}"/> and on map representations <see cref="IMapRepre"/>.
/// </summary>
public abstract class BlankGraphElevDataIndepBlankTemplateTextMapImplementation :
    IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    /// <inheritdoc cref="IGraph{TVertexAttributes,TEdgeAttributes}.RestoreConsistency"/>
    public void RestoreConsistency() { }

    /// <inheritdoc cref="IMapRepre.Name"/>
    public string Name { get; } = "Blank map representation.";
}
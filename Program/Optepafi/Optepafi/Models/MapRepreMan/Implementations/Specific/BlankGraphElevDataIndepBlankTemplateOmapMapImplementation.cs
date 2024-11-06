using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific;

//TODO: comment
public class BlankGraphElevDataIndepBlankTemplateOmapMapImplementation :
    IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    /// <inheritdoc cref="IGraph{TVertexAttributes,TEdgeAttributes}.RestoreConsistency"/>
    public void RestoreConsistency() { }

    /// <inheritdoc cref="IMapRepre.Name"/>
    public string Name { get; } = "Blank map representation.";
    
}
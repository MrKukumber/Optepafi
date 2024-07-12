using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

/// <summary>
/// Blank graph. It will contain no bojects representing provided map, nor implement any functionality at all.
/// It is just blank object used as demonstrative type for presenting application functionality.
/// For more information about graphs see <see cref="IGraph{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes that are provided in vertices of generated graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes that are provided in edges of generated graph.</typeparam>
public interface IBlankGraph<TVertexAttributes, TEdgeAttributes> :
    IBlankRepre,
    IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
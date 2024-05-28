using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;

public interface IBlankGraph<TVertexAttribtues, TEdgeAttributes> : 
    IBlankRepre,
    IGraph<TVertexAttribtues, TEdgeAttributes>
    where TVertexAttribtues : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
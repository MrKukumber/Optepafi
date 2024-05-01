using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ISearchableReferringMapRepre<out TTemplate, TVertexAttribtues, TEdgeAttributes> : 
    ISearchableMapRepre<TTemplate, BasicVertex<TVertexAttribtues, TEdgeAttributes>, BasicEdge<TEdgeAttributes,TVertexAttribtues>, TVertexAttribtues, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttribtues, TEdgeAttributes>
    where TVertexAttribtues : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
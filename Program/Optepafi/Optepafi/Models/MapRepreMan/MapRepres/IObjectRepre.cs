using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;



public interface IObjectRepre<out TTemplate, TVertexAttributes, TEdgeAttributes> :
    IPredecessorRememberingReferringMapRepre<TTemplate, TVertexAttributes, TEdgeAttributes>,
    IAStarHeuristicEnsuringMapRepre<BasicVertex<TVertexAttributes, TEdgeAttributes>, TVertexAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

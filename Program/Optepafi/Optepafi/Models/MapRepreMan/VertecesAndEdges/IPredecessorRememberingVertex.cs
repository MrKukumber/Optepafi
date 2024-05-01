using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Verteces;

public class PredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> :
    BasicVertex<TVertexAttributes, TEdgeAttributes>,
    IPredecessorRememberingVertex<BasicEdge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

public interface IPredecessorRememberingVertex<TBasicEdge, TVertexAttributes> : 
    IBasicVertex<TBasicEdge, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TBasicEdge : IEdge
{
    IPredecessorRememberingVertex<TBasicEdge, TVertexAttributes>? Predecessor { get; set; }
}

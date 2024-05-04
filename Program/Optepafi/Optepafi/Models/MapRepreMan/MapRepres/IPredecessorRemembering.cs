using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IPredecessorRemembering<out TTemplate, out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes> :
    ISearchable<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertex : IPredecessorRememberingVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    TVertex ISearchable<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>.GetVertexFor((int, int) coords)
    {
        return GetVertexFor(coords);
    }
    new TVertex GetVertexFor((int, int) coords);
}

public interface IPredecessorRememberingReferring<out TTemplate, TVertexAttributes, TEdgeAttributes> :
    IPredecessorRemembering<TTemplate, IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
}
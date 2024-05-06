using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;

public interface IPredecessorRemembering<out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes> :
    ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
    where TVertex : IPredecessorRememberingVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    TVertex ISearchable<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>.GetVertexFor((int, int) coords)
    {
        return GetVertexFor(coords);
    }
    new TVertex GetVertexFor((int, int) coords);
}

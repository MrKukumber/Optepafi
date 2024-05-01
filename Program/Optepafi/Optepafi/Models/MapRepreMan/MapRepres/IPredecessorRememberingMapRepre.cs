using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IPredecessorRememberingMapRepre<out TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes> :
    ISearchableMapRepre<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertex : IPredecessorRememberingVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    TVertex ISearchableMapRepre<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>.GetVertexFor((int, int) coords)
    {
        return GetVertexFor(coords);
    }
    new TVertex GetVertexFor((int, int) coords);
}

public interface IPredecessorRememberingReferringMapRepre<out TTemplate, TVertexAttributes, TEdgeAttributes> :
    ISearchableReferringMapRepre<TTemplate, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
    BasicVertex<TVertexAttributes, TEdgeAttributes> ISearchableMapRepre<TTemplate, BasicVertex<TVertexAttributes, TEdgeAttributes>, BasicEdge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes, TEdgeAttributes>.GetVertexFor((int, int) coords)
    {
        return GetVertexFor(coords);
    }
    new PredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> GetVertexFor((int, int) coords);
}
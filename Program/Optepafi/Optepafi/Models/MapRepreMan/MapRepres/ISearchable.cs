using Avalonia.Controls;
using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ISearchable<out TTemplate, out TVertex, out TEdge, out  TVertexAttributes, out TEdgeAttributes> :
    IMapRepreFunctionality<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertex : IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    TVertex GetVertexFor((int, int) coords);//TODO: mozno vytvorit reprezentaciu pre coords, nejaky struct
}

public interface ISearchableReferring<out TTemplate, TVertexAttribtues, TEdgeAttributes> : 
    ISearchable<TTemplate, IBasicEdgeCoupledBasicVertex<TVertexAttribtues, TEdgeAttributes>, IBasicEdge<IBasicEdgeCoupledBasicVertex<TVertexAttribtues, TEdgeAttributes>, TEdgeAttributes>, TVertexAttribtues, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttribtues, TEdgeAttributes>
    where TVertexAttribtues : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

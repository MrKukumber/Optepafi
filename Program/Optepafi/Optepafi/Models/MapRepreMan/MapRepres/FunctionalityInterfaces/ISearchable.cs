using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;

public interface ISearchable<out TVertex, out TEdge, out  TVertexAttributes, out TEdgeAttributes>
    where TVertex : IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    TVertex GetVertexFor((int, int) coords);//TODO: mozno vytvorit reprezentaciu pre coords, nejaky struct
}


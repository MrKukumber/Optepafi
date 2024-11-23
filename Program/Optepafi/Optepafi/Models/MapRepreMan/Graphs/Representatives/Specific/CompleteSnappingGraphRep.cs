using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

//TODO: comment
public class CompleteSnappingGraphRep<TVertexAttributes, TEdgeAttributes> : GraphRepresentative<ICompleteSnappingGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static CompleteSnappingGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private CompleteSnappingGraphRep() { }

}
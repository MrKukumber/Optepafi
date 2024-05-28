using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Representatives.GraphRepresentatives;

public class BlankGraphRep<TVertexAttributes, TEdgeAttributes> : IGraphRepresentative<IBlankGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static BlankGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private BlankGraphRep() { }
}
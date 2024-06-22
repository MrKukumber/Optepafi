using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Representatives.GraphRepresentatives;

public class ObjectRepreGraphRep<TVertexAttributes, TEdgeAttributes> :
    IGraphRepresentative<IObjectRepreGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static ObjectRepreGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private ObjectRepreGraphRep() { }
}
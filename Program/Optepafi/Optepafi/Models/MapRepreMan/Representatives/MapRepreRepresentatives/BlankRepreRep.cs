using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreImplementationReps.SpecificImplementationReps;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.Representatives.GraphRepresentatives;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreReps.MapRepreRepsInterfaces;

public class BlankRepreRep : IMapRepreRepresentative<IBlankRepre>
{
    public static BlankRepreRep Instance { get; } = new();
    private BlankRepreRep(){}
    
    public string MapRepreName { get; } = "Blank representation.";
    public IImplementationIndicator<ITemplate, IMap, IBlankRepre>[] ImplementationIndicators { get; } = [BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep.Instance];
    public IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>() where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return BlankGraphRep<TVertexAttributes, TEdgeAttributes>.Instance;
    }
}
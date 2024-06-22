using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;


public class ObjectRepreRep : IMapRepreRepresentative<IObjectRepre>
{
    public static ObjectRepreRep Instance { get; } = new ();
    private ObjectRepreRep(){}
    
    public string MapRepreName { get; } = ""; //TODO: vymysliet pekne meno
    public IImplementationIndicator<ITemplate, IMap, IObjectRepre>[] ImplementationIndicators { get; } = { };

    public IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
        GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return ObjectRepreGraphRep<TVertexAttributes, TEdgeAttributes>.Instance;
    }
}

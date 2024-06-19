using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepreImplementationReps;
using Optepafi.Models.MapRepreMan.MapRepreImplementationReps.SpecificImplementationReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.Representatives.GraphRepresentatives;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreReps.MapRepreRepsInterfaces;

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

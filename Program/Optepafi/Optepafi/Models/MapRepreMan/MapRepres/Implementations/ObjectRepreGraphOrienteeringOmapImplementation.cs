using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.GraphImplementations;

public class ObjectRepreGraphOrienteeringOmapImplementation : 
    IObjectRepreGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    //TODO: implement
    public void RestoreConsistency()
    {
        throw new System.NotImplementedException();
    }

    public string Nmae { get; } = "Object representation";

    public IMapRepreRepresentative<IMapRepre> MapRepreRep { get; init; }

    public ObjectRepreGraphOrienteeringOmapImplementation()
    {
    }

    public IBasicEdgeCoupledPredecessorRememberingVertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> GetVertexFor((int, int) coords)
    {
        throw new System.NotImplementedException();
    }

    public int GetWeightFromHeuristic(IBasicEdgeCoupledPredecessorRememberingVertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> from,
        IBasicEdgeCoupledPredecessorRememberingVertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> to)
    {
        throw new System.NotImplementedException();
    }
}
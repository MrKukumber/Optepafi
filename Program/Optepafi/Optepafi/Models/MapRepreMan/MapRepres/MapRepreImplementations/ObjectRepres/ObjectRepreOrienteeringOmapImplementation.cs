using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.MapRepreImplementations.ObjectRepres;

public class ObjectRepreOrienteeringOmapImplementation : 
    IFunctionalObjectRepre<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    //TODO: implement
    public void RestoreConsistency()
    {
        throw new System.NotImplementedException();
    }

    public IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }

    public ObjectRepreOrienteeringOmapImplementation()
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
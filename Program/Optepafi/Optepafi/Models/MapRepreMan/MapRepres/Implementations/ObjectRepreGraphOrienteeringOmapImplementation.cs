using System.Collections.Generic;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.GraphImplementations;

public class ObjectRepreGraphOrienteeringOmapImplementation : 
    IObjectRepreGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    //TODO: implement

    public string Name { get; } = "Object representation";

    public ObjectRepreGraphOrienteeringOmapImplementation()
    {
    }

    public void RestoreConsistency()
    {
        throw new System.NotImplementedException();
    }
    public IBasicEdgeCoupledPredecessorRememberingVertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> 
        GetVertexFor(MapCoordinate coords)
    {
        throw new System.NotImplementedException();
    }
}
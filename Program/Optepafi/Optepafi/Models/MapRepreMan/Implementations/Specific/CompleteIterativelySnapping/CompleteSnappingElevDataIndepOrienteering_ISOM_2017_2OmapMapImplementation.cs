using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

//TODO: comment
public class CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation : 
    IGraph<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>>, 
    ICompleteSnappingGraph
{
    public string Name { get; } = "Complete snapping map repre. elev. data independent implementation for Orienteering (ISOM 2017-2) and Omap file format.";
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit<CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation>(this, otherParams);
    public void RestoreConsistency()
    {
        throw new System.NotImplementedException();
    }
    public ICompleteSnappingGraph.Vertex<IVertexAttributes, IEdgeAttributes> GetVertexFor(MapCoordinates coords)
    {
        throw new System.NotImplementedException();
    }
}
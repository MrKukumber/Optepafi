using System.IO;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

//TODO: comment
public class CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(INearestNeighborsSearchableDataStructure<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>> searchableVertices, CompleteSnappingMapRepreConfiguration configuration) : 
    IGraph<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>>, 
    ICompleteSnappingGraph
{
    public string Name { get; } = "Complete snapping map repre. elev. data independent implementation for Orienteering (ISOM 2017-2) and Omap file format.";
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit<CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation>(this, otherParams);
    public void RestoreConsistency()
    {
        foreach (var vertex in searchableVertices)
        {
            vertex.Predecessor = null;
            foreach (var edge in vertex.GetEdges())
            {
                vertex.SetWeight(null, edge);
            }
        }
    }
    public ICompleteSnappingGraph.Vertex<IVertexAttributes, IEdgeAttributes> GetVertexFor(MapCoordinates coords)
    {
        var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
        if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
        return new ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>(new Orienteering_ISOM_2017_2.VertexAttributes(coords, 0), [new ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>( new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), nearestVertices[0])]);
    }
    //TODO: pridat scale mapy
}
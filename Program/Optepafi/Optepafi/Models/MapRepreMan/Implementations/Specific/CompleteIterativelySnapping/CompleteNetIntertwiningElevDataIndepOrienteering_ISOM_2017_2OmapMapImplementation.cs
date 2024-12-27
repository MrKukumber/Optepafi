using System.Collections;
using System.Collections.Generic;
using System.IO;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

//TODO: comment
public class CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(INearestNeighborsSearchableDataStructure<ICompleteNetIntertwiningGraph< Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> searchableVertices, int scale) : 
    ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, IEnumerable<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>
{
    public string Name { get; } = "Complete snapping map repre. elev. data independent implementation for Orienteering (ISOM 2017-2) and Omap file format.";

    // public IEditableRadiallySearchableDataStruct<ICompleteNetIntertwiningGraph< Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> SearchableVertices { get; } = searchableVertices; //for debugging

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
    public IBasicEdgeCoupledPredecessorRememberingVertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> GetVertexFor(MapCoordinates coords)
    {
        var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
        if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
        return new ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(new Orienteering_ISOM_2017_2.VertexAttributes(coords, 0), [new ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), nearestVertices[0])]);
    }

    public int Scale { get; } = scale;
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation>(this, otherParams);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> GetEnumerator() => searchableVertices.GetEnumerator();
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

//TODO: comment
public class CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(INearestNeighborsSearchableDataStructure<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex> searchableVertices, int scale) : 
    ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, 
    IEnumerable<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>
{
    public string Name { get; } = "Complete snapping map repre. elev. data independent implementation for Orienteering (ISOM 2017-2) and Omap file format.";

    // public IEditableRadiallySearchableDataStruct<ICompleteNetIntertwiningGraph< Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> SearchableVertices { get; } = searchableVertices; //for debugging

    public void RestoreConsistency()
    {
        foreach (var (graphVertexConnectedToNewVertex, connectingEdge) in _graphVerticesConnectedToNewVerticesByEdge)
        {
            graphVertexConnectedToNewVertex.RemoveEdge(connectingEdge);
        }
        _graphVerticesConnectedToNewVerticesByEdge.Clear();
        foreach (var vertex in searchableVertices)
        {
            vertex.Predecessor = null;
            foreach (var edge in vertex.GetEdges())
                vertex.SetWeight(float.NaN, edge);
        }
    }

    private List<(EdgesEditableVertex, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes,Orienteering_ISOM_2017_2.EdgeAttributes>.Edge)> _graphVerticesConnectedToNewVerticesByEdge = [];
    public ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex GetVertexFor(MapCoordinates coords)
    {
        var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
        if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
        var newVertex = new ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(new Orienteering_ISOM_2017_2.VertexAttributes(coords), [new ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), nearestVertices[0])]);
        var edgeToNewVertex = new ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes( (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), newVertex);
        nearestVertices[0].AddEdge(edgeToNewVertex);
        _graphVerticesConnectedToNewVerticesByEdge.Add((nearestVertices[0], edgeToNewVertex)); 
        return newVertex;
    }
    
    public int Scale { get; } = scale;

    public TOut AcceptGeneric<TOut, TGenericParam1, TGenericParam2, TGenericConstraint1, TGenericConstraint2, TOtherParams>(IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams> genericVisitor, TOtherParams otherParams)
        where TGenericParam1 : TGenericConstraint1 where TGenericParam2 : TGenericConstraint2
        => genericVisitor.GenericVisit<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex,ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, TGenericParam1, TGenericParam2>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(ICanBeSearchedGenericVisitor<TOut, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor .GenericVisit<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(IRemembersPredecessorsGenericVisitor<TOut, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams) 
        => genericVisitor.GenericVisit<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> GetEnumerator() => searchableVertices.GetEnumerator();

    public abstract class EdgesEditableVertex(Orienteering_ISOM_2017_2.VertexAttributes attributes, IEnumerable<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge> outgoingEdges) : ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(attributes, outgoingEdges)
    {
        public abstract void AddEdge(ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);

        public abstract void RemoveEdge(ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);
    }
}
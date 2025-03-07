using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

public class CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(INearestNeighborsSearchableDataStructure<CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex> searchableVertices, int scale) :
    ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, 
    IEnumerable<ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>
{
    public string Name { get; } = "Complete net intertwining map repre. elev. data independent basic implementation for Orienteering (ISOM 2017-2) and Omap file format.";

    public void RestoreConsistency()
    {
        foreach (var (graphVertexConnectedToNewVertex, connectingEdge) in _graphVerticesConnectedToNewVerticesByEdge)
        {
            graphVertexConnectedToNewVertex.RemoveEdge(connectingEdge);
        }
        _graphVerticesConnectedToNewVerticesByEdge.Clear();
        foreach (var vertex in searchableVertices)
        {
            foreach (var edge in vertex.GetEdges())
                edge.SetWeight(float.NaN);
        }
    }
    
    private List<(EdgesEditableVertex, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes,Orienteering_ISOM_2017_2.EdgeAttributes>.Edge)> _graphVerticesConnectedToNewVerticesByEdge = [];

    public ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex GetVertexFor(MapCoordinates coords)
    {
        var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
        if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
        var newVertex = new ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(new Orienteering_ISOM_2017_2.VertexAttributes(coords), [new ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), nearestVertices[0])]);
        var edgeToNewVertex = new ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes( (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), newVertex);
        nearestVertices[0].AddEdge(edgeToNewVertex);
        _graphVerticesConnectedToNewVerticesByEdge.Add((nearestVertices[0], edgeToNewVertex)); 
        return newVertex;
    }
    
    public int Scale { get; } = scale;
    
    public TOut AcceptGeneric<TOut, TGenericParam1, TGenericParam2, TGenericConstraint1, TGenericConstraint2, TOtherParams>(
        IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams> genericVisitor, TOtherParams otherParams) where TGenericParam1 : TGenericConstraint1 where TGenericParam2 : TGenericConstraint2
        => genericVisitor.GenericVisit<CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex,ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, TGenericParam1, TGenericParam2>(this, otherParams);

    public TOut AcceptGeneric<TOut, TOtherParams>(ICanBeSearchedGenericVisitor<TOut, float, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit<CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit(this, otherParams);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> GetEnumerator() => searchableVertices.GetEnumerator();

    public abstract class EdgesEditableVertex(Orienteering_ISOM_2017_2.VertexAttributes attributes, IEnumerable<ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge> outgoingEdges) : ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(attributes, outgoingEdges)
    {
        public abstract void AddEdge(ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);
        public abstract void RemoveEdge(ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);
    }
}
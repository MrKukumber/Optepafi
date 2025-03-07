using System.Collections;
using System.Collections.Generic;
using System.IO;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

/// <summary>
/// Complete net intertwining graph implementation of OMAP map representation which uses <see cref="Orienteering_ISOM_2017_2"/>
/// tempalte and does not require elevation data for its creation. It does not hold any elevation information at all.
/// </summary>
public class CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(INearestNeighborsSearchableDataStructure<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex> searchableVertices, int scale) : 
    ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, 
    IEnumerable<ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>
{
    public string Name { get; } = "Complete net intertwining map repre. elev. data independent predecessors remembering implementation for Orienteering (ISOM 2017-2) and Omap file format.";

    // public INearestNeighborsSearchableDataStructure<ICompleteNetIntertwiningGraph< Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> SearchableVertices { get; } = searchableVertices; //for debugging

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
                edge.SetWeight(float.NaN);
        }
    }

    private List<(EdgesEditableVertex, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes,Orienteering_ISOM_2017_2.EdgeAttributes>.Edge)> _graphVerticesConnectedToNewVerticesByEdge = [];
    public ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex GetVertexFor(MapCoordinates coords)
    {
        var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
        if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
        var newVertex = new ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(new Orienteering_ISOM_2017_2.VertexAttributes(coords), [new ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), nearestVertices[0])]);
        var edgeToNewVertex = new ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge( new Orienteering_ISOM_2017_2.EdgeAttributes( (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), newVertex);
        nearestVertices[0].AddEdge(edgeToNewVertex);
        _graphVerticesConnectedToNewVerticesByEdge.Add((nearestVertices[0], edgeToNewVertex)); 
        return newVertex;
    }
    
    public int Scale { get; } = scale;

    public TOut AcceptGeneric<TOut, TGenericParam1, TGenericParam2, TGenericConstraint1, TGenericConstraint2, TOtherParams>(IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams> genericVisitor, TOtherParams otherParams)
        where TGenericParam1 : TGenericConstraint1 where TGenericParam2 : TGenericConstraint2
        => genericVisitor.GenericVisit<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex,ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, TGenericParam1, TGenericParam2>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(ICanBeSearchedGenericVisitor<TOut, float, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(IRemembersPredecessorsGenericVisitor<TOut, float, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes, TOtherParams> genericVisitor, TOtherParams otherParams) 
        => genericVisitor.GenericVisit<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>(this, otherParams);
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit(this, otherParams);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex> GetEnumerator() => searchableVertices.GetEnumerator();

    public abstract class EdgesEditableVertex(Orienteering_ISOM_2017_2.VertexAttributes attributes, IEnumerable<ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge> outgoingEdges) 
        : ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex(attributes, outgoingEdges)
    {
        public abstract void AddEdge(ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);
        public abstract void RemoveEdge(ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge newEdge);
    }
}
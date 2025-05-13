using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

/// <summary>
/// Represents and creates net intertwining graph implementation of OMAP map representation which uses <see cref="Orienteering_ISOM_2017_2"/>
/// tempalte and does not require elevation data for its creation. It does not include any elevation information in the graph at all.
/// </summary>
public class CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep : 
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, CompleteNetIntertwiningMapRepreConfiguration, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }

    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation CreateImplementation(Orienteering_ISOM_2017_2 template, OmapMap map,
        CompleteNetIntertwiningMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        var vertexBuilders = new GraphCreator<BuildableVertex, BuildableEdge >().Create(map, configuration, progress, cancellationToken);
        RadiallySearchableKdTree<BuildableVertex> vertices = new (vertexBuilders.Select(vb => vb.Build()), v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos));
        foreach (var vertexBuilder in vertexBuilders) vertexBuilder.ConnectAfterBuild(); 
        return new CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapInnerImplementation(vertices, map.Scale);
    }
    
    private class BuildableVertex : 
        ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex,
        IInitializableVertex<BuildableEdge, Orienteering_ISOM_2017_2.VertexAttributes>,
        IBuildableVertex<BuildableEdge>

    {
        public BuildableVertex() : base() { }
        public IList<BuildableEdge> OutgoingEdges { get => (IList<BuildableEdge>) _outgoingEdges; init => _outgoingEdges = value; }
        public new Orienteering_ISOM_2017_2.VertexAttributes Attributes { get => _attributes; init => _attributes = value; }
        public void AddEdge(BuildableEdge edge) => OutgoingEdges.Add(edge);
    }
    
    private class BuildableEdge : 
        ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge,
        IInitializableEdge<BuildableVertex, Orienteering_ISOM_2017_2.EdgeAttributes>
    {
        public BuildableEdge() : base() {}
        public Orienteering_ISOM_2017_2.EdgeAttributes Attributes { get => _attributes; init => _attributes = value; }
        public BuildableVertex To { get => (BuildableVertex) _to; init => _to = value; }
    }

    private class CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapInnerImplementation( INearestNeighborsSearchableDataStructure<BuildableVertex> searchableVertices, int scale)
        : CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation( searchableVertices, scale)
    {
        public override void RestoreConsistency()
        {
            foreach (var (graphVertexConnectedToNewVertex, connectingEdge) in _graphVerticesConnectedToNewVerticesByEdge)
            {
                graphVertexConnectedToNewVertex.OutgoingEdges.Remove(connectingEdge);
            }
            _graphVerticesConnectedToNewVerticesByEdge.Clear();
            foreach (var vertex in searchableVertices)
            {
                vertex.Predecessor = null;
                foreach (var edge in vertex.GetEdges())
                    edge.SetWeight(float.NaN);
            }
        }

        private List<(BuildableVertex, BuildableEdge)> _graphVerticesConnectedToNewVerticesByEdge = [];
        
        public override ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex GetVertexFor(MapCoordinates coords)
        {
            var nearestVertices = searchableVertices.GetNearestNeighbors((coords.XPos, coords.YPos), 1);
            if (nearestVertices.Length == 0) throw new InvalidDataException("No vertices in graph found."); 
            var newVertex = new BuildableVertex{ Attributes = new Orienteering_ISOM_2017_2.VertexAttributes(coords), OutgoingEdges = [new BuildableEdge(){Attributes = new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), To =nearestVertices[0]}]};
            var edgeToNewVertex = new BuildableEdge{ Attributes = new Orienteering_ISOM_2017_2.EdgeAttributes( (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), (null, null, null)), To = newVertex};
            nearestVertices[0].OutgoingEdges.Add(edgeToNewVertex);
            _graphVerticesConnectedToNewVerticesByEdge.Add((nearestVertices[0], edgeToNewVertex)); 
            return newVertex;
        }
    }

}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia.Controls.Primitives;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Configurations;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;
using Optepafi.Models.Utils.Shapes.Segments;
using Priority_Queue;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations.AStar;

//TODO: comment
public class AStarGeneral : 
    ISearchingAlgorithmImplementation<AStarConfiguration>
{
    public static AStarGeneral Instance { get; } = new AStarGeneral();
    private AStarGeneral() {}
    public bool DoesRepresentUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge> graphRepresentative) 
        where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return graphRepresentative is IGraphRepresentative<IRemembersPredecessors<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>, TVertex, TEdge>;
    }

    public bool DoesRepresentUsableUserModel<TTemplate, TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> userModelType) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return userModelType 
            is IUserModelType<IAStarHeuristicsComputing<TTemplate, TVertexAttributes, TEdgeAttributes> , TTemplate> 
            and IUserModelType<IWeightComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> ;
    }

    public bool IsUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(IGraph<TVertex, TEdge> graph) 
        where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return graph is IRemembersPredecessors<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>;
    }

    public bool IsUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return userModel 
            is IAStarHeuristicsComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> 
            and IWeightComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>;
    }

    public IPath<TVertexAttributes, TEdgeAttributes>[] SearchForPaths<TVertexAttributes, TEdgeAttributes, TVertex, TEdge>(Leg[] track, IGraph<TVertex, TEdge> graph, IList<IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>> userModels,
        AStarConfiguration configuration, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes where TVertex : IVertex where TEdge : IEdge
    {
        List<IPath<TVertexAttributes, TEdgeAttributes>> paths = new();
        foreach (var userModel in userModels)
        {
            paths.Add(ExecutorSearch(track, graph, userModel, configuration, progress, cancellationToken));
            graph.RestoreConsistency();
        }
        return paths.ToArray();
    }

    public IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes, TVertex, TEdge>(
        Leg[] track, IGraph<TVertex, TEdge> graph,
        IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel, AStarConfiguration configuration,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
        where TVertex : IVertex
        where TEdge : IEdge
        => Algorithm<TVertexAttributes, TEdgeAttributes>.Instance.Search(track, graph, userModel, configuration, progress, cancellationToken); 

    private class Algorithm<TVertexAttributes, TEdgeAttributes> :
        IRemembersPredecessorsGenericVisitor<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes, (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken? )> 
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        public static Algorithm<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
        private Algorithm() { }
        private const int CancellationCheckDensity = 5000;
        
        public IPath<TVertexAttributes, TEdgeAttributes> Search<TVertex, TEdge>(Leg[] track, IGraph<TVertex, TEdge> graph, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel, AStarConfiguration configuration, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertex : IVertex
        where TEdge : IEdge
        {
            if (graph is IRemembersPredecessors<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
                graphWhichRemembersPredecessors)
            {
                SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> path = new();
                foreach (var leg in track)
                {
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return path;
                    path.MergeWith(graphWhichRemembersPredecessors.AcceptGeneric(this, (leg, userModel, configuration, progress, cancellationToken)));
                }
                return path;
            }
            throw new ArgumentException("Graph was not convertible to IRemembersPredecessorsGenericVisitor even though it should be checked before call of this method.");
        }

        SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> IRemembersPredecessorsGenericVisitor<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes, (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken? )>.
            GenericVisit<TPredecessorRemembering, TVertex, TEdge>(TPredecessorRemembering predecessorRememberingGraph,
            (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken?) otherParams)
        {
            var (leg, userModel, configuration, progress, cancellationToken) = otherParams;
            if (userModel is IWeightComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> weightComputingUserModel
                && userModel is IAStarHeuristicsComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> aStarHeuristicsComputingUserModel)
            {
                TVertex startVertex = predecessorRememberingGraph.GetVertexFor(leg.Start);
                TVertex finishVertex = predecessorRememberingGraph.GetVertexFor(leg.Finish);
                SimplePriorityQueue<TVertex, float> frontier = new();
                Dictionary<TVertex, float> frontierVerticesCosts = new();
                HashSet<TVertex> visited = new();
                frontier.Enqueue(startVertex, aStarHeuristicsComputingUserModel.GetWeightFromHeuristics(startVertex.Attributes, finishVertex.Attributes) + 0);
                frontierVerticesCosts.Add(startVertex, 0);
                int timeSinceLastCancellationCheck = 0;
                while (frontier.TryDequeue(out TVertex vertex))
                {
                    if (++timeSinceLastCancellationCheck >= CancellationCheckDensity && cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                        break;
                    visited.Add(vertex);
                    if (vertex.Equals(finishVertex))
                        return GetPathToFinish(vertex);
                    foreach (TEdge edge in vertex.GetEdges())
                        if (!visited.Contains(edge.To))
                        {
                            float g = frontierVerticesCosts[vertex] + GetEdgeWeight(vertex, edge, weightComputingUserModel);
                            float f = g + aStarHeuristicsComputingUserModel.GetWeightFromHeuristics(edge.To.Attributes, finishVertex.Attributes);
                            if (frontier.Contains(edge.To))
                                if (frontier.GetPriority(edge.To) < f) continue;
                                else frontier.UpdatePriority(edge.To, f);
                            else frontier.Enqueue(edge.To, f);
                            frontierVerticesCosts[edge.To] = g;
                            edge.To.Predecessor = vertex;
                        }
                    frontierVerticesCosts.Remove(vertex);
                }
                return new SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>();
            }
            throw new ArgumentException("User model does not provided required functionality.");
        }

        private float GetEdgeWeight<TVertex, TEdge>(TVertex vertex, TEdge edge, IWeightComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> weightComputingUserModel)
            where TVertex : IPredecessorRememberingVertex<TVertexAttributes>, IBasicVertex<TEdge, TVertexAttributes>
            where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
        {
            if (vertex.TryGetWeight(edge, out float weight))
                return weight; 
            var computedWeight = weightComputingUserModel.ComputeWeight(vertex.Attributes, edge.Attributes, edge.To.Attributes);
            vertex.SetWeight(computedWeight, edge);
            return computedWeight;
        }

        private SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> GetPathToFinish(IPredecessorRememberingVertex<TVertexAttributes>? lastVertex)
        {
            List<TVertexAttributes> reversedPathCoordinates = new();
            while (lastVertex != null)
            {
                reversedPathCoordinates.Add(lastVertex.Attributes);
                lastVertex = lastVertex.Predecessor;
            }
            reversedPathCoordinates.Reverse();
            return new SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>(reversedPathCoordinates);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
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
using Priority_Queue;
using Tmds.DBus.Protocol;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations.AStar;

public class AStarUsesPredsDict : ISearchingAlgorithmImplementation<AStarConfiguration>
{
    
    public static AStarUsesPredsDict Instance { get; } = new AStarUsesPredsDict();
    private AStarUsesPredsDict() {}
    public bool DoesRepresentUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(
        IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge> graphRepresentative) where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return graphRepresentative is IGraphRepresentative< ICanBeSearched<TVertex, TEdge, float, TVertexAttributes, TEdgeAttributes>, TVertex, TEdge>;
    }

    public bool DoesRepresentUsableUserModel<TTemplate, TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> userModelType) where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return userModelType 
            is IUserModelType<IAStarHeuristicsComputing<TTemplate, TVertexAttributes, TEdgeAttributes> , TTemplate> 
            and IUserModelType<IWeightComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> ;
    }

    public bool IsUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(IGraph<TVertex, TEdge> graph) where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return graph is ICanBeSearched<TVertex, TEdge, float, TVertexAttributes, TEdgeAttributes>;
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

    public IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes, TVertex, TEdge>(Leg[] track, IGraph<TVertex, TEdge> graph,
        IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel, AStarConfiguration configuration, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes where TVertex : IVertex where TEdge : IEdge
        => Algorithm<TVertexAttributes, TEdgeAttributes>.Instance.Search(track, graph, userModel, configuration, progress, cancellationToken); 
    
    private class Algorithm<TVertexAttributes, TEdgeAttributes> :
        ICanBeSearchedGenericVisitor<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, float, TVertexAttributes, TEdgeAttributes, (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken? )> 
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
            if (graph is ICanBeSearched<TVertex, TEdge, float, TVertexAttributes, TEdgeAttributes>
                searchableGraph)
            {
                SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> path = new();
                foreach (var leg in track)
                {
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return path;
                    path.MergeWith(searchableGraph.AcceptGeneric(this, (leg, userModel, configuration, progress, cancellationToken)));
                }
                return path;
            }
            throw new ArgumentException("Graph was not convertible to IRemembersPredecessorsGenericVisitor even though it should be checked before call of this method.");
        }

        SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> ICanBeSearchedGenericVisitor<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, float, TVertexAttributes, TEdgeAttributes, (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken? )>.
            GenericVisit<TPredecessorRemembering, TVertex, TEdge>(TPredecessorRemembering basicGraph,
            (Leg, IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>, AStarConfiguration, IProgress<ISearchingReport>?, CancellationToken?) otherParams)
        {
            var (leg, userModel, configuration, progress, cancellationToken) = otherParams;
            if (userModel is IWeightComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> weightComputingUserModel
                && userModel is IAStarHeuristicsComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> aStarHeuristicsComputingUserModel)
            {
                TVertex startVertex = basicGraph.GetVertexFor(leg.Start);
                TVertex finishVertex = basicGraph.GetVertexFor(leg.Finish);
                SimplePriorityQueue<TVertex, float> frontier = new();
                Dictionary<TVertex, float> frontierVerticesCosts = new();
                HashSet<TVertex> visited = new();
                Dictionary<TVertex, TVertex> predecessors = new();
                frontier.Enqueue(startVertex, aStarHeuristicsComputingUserModel.GetWeightFromHeuristics(startVertex.Attributes, finishVertex.Attributes) + 0);
                frontierVerticesCosts.Add(startVertex, 0);
                int timeSinceLastCancellationCheck = 0;
                while (frontier.TryDequeue(out TVertex vertex))
                {
                    if (++timeSinceLastCancellationCheck >= CancellationCheckDensity && cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                        break;
                    visited.Add(vertex);
                    if (vertex.Equals(finishVertex))
                        return GetPathToFinish<TVertex, TEdge>(vertex, predecessors);
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
                            predecessors[edge.To] = vertex;
                        }
                    frontierVerticesCosts.Remove(vertex);
                }
                return new SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>();
            }
            throw new ArgumentException("User model does not provided required functionality.");
        }

        private float GetEdgeWeight<TVertex, TEdge>(TVertex vertex, TEdge edge, IWeightComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> weightComputingUserModel)
            where TVertex : IBasicVertex<TEdge, TVertexAttributes>
            where TEdge : IBasicEdge<TVertex, TEdgeAttributes, float>
        {
            if (edge.GetWeight() is not float.NaN)
                return edge.GetWeight(); 
            var computedWeight = weightComputingUserModel.ComputeWeight(vertex.Attributes, edge.Attributes, edge.To.Attributes);
            edge.SetWeight(computedWeight);
            return computedWeight;
        }

        private SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> GetPathToFinish<TVertex, TEdge >(TVertex lastVertex, Dictionary<TVertex, TVertex> predecessors)
            where TVertex : IBasicVertex<TEdge, TVertexAttributes>
            where TEdge : IBasicEdge<TVertex, TEdgeAttributes, float>
        {
            List<TVertexAttributes> reversedPathCoordinates = new();
            while (lastVertex != null)
            {
                reversedPathCoordinates.Add(lastVertex.Attributes);
                lastVertex = predecessors[lastVertex];
            }
            reversedPathCoordinates.Reverse();
            return new SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>(reversedPathCoordinates);
        }
    }
}
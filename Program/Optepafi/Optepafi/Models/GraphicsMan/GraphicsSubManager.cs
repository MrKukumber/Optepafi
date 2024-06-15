using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.Graphics.GraphicsAggregators;
using Optepafi.Models.Graphics.GraphicsAggregators.Path;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.Graphics;

public class GraphicsSubManager<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public IReadOnlySet<IGraphicsAggregator> PathGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(SmileyFaceGraphicsAggregator<TVertexAttributes, TEdgeAttributes>.Instance);
    public IReadOnlySet<IGraphicsAggregator> SearchingStateGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>();
    
    public static GraphicsSubManager<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private GraphicsSubManager(){}
    
    public enum AggregationResult {Aggregated, NoUsableAggregatorFound}

    public AggregationResult AggregatePathGraphics<TPath>(TPath path,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
        where TPath : IPath<TVertexAttributes, TEdgeAttributes>
    {
        foreach (var graphicsAggregator in PathGraphicsAggregators)
        {
            if (graphicsAggregator is IPathGraphicsAggregator<TPath, TVertexAttributes, TEdgeAttributes> pathGraphcisAggregator)
            {
                pathGraphcisAggregator.AggregateGraphics(path, userModel, collectorForAggregatedObjects, cancellationToken);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }

    public AggregationResult AggregateSearchingStateGraphics<TSearchingState>(TSearchingState searchingState,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
        where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes>
    {
        foreach (var graphicsAggregator in SearchingStateGraphicsAggregators)
        {
            if (graphicsAggregator is
                ISearchingStateGraphicsAggregator<TSearchingState, TVertexAttributes, TEdgeAttributes>
                searchingStateGraphicsAggregator)
            {
                searchingStateGraphicsAggregator.AggregateGraphics(searchingState, userModel, collectorForAggregatedObjects, cancellationToken);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }
    

    // public List<AggregationResult> AggregatePathsGraphics<TPath>(TPath[] paths,
        // IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        // IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
        // where TPath : IPath<TVertexAttributes, TEdgeAttributes>
    // {
        
        // List<AggregationResult> results = new List<AggregationResult>();
        // foreach (var path in paths)
        // {
            // if(cancellationToken?.IsCancellationRequested ?? false) 
                // results.Add(AggregationResult.Cancelled);
            // else
                // results.Add(AggregatePathGraphics(path, userModel, collectorForAggregatedObjects));
        // }
        // return results;
    // }
}
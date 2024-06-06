using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.Graphics.GraphicsAggregators.MapGraphicsAggregators;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.ModelViews.Graphics;
using Optepafi.ModelViews.Graphics.Collectors;

namespace Optepafi.Models.Graphics;

/// <summary>
/// 
/// </summary>
public class GraphicsManager :
    IMapGenericVisitor<GraphicsManager.AggregationResult, (IGraphicsObjectCollector, CancellationToken?)>,
    IMapGenericVisitor<(int, int, int, int)?>,
    IPathGenericVisitor<GraphicsManager.AggregationResult, IGraphicsObjectCollector>
{
    public static GraphicsManager Instance { get; } = new();
    private GraphicsManager() { }

    public IReadOnlySet<IGraphicsAggregator> MapGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(TextMapGraphicsAggregator.Instance);

    public IReadOnlySet<IGraphicsAggregator> PathGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>();
    
    public enum AggregationResult {Aggregated, NoUsableAggregatorFound, Cancelled}

    public AggregationResult AggregateMapGraphics(IMap map, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric(this, (collectorForAggregatedObjects, cancellationToken));
    }
    
    AggregationResult IMapGenericVisitor<AggregationResult, (IGraphicsObjectCollector, CancellationToken?)>.GenericVisit<TMap>(TMap map, (IGraphicsObjectCollector, CancellationToken?) otherParams)
    {
        var (collectorForAggregatedObjects, cancellationToken) = otherParams;
        foreach (var graphicsAggregator in MapGraphicsAggregators)
        {
            if (graphicsAggregator is IMapGraphicsAggregator<TMap> mapGraphcisAggregator)
            {
                mapGraphcisAggregator.AggregateGraphics(map, collectorForAggregatedObjects, cancellationToken);
                if (cancellationToken?.IsCancellationRequested ?? false)
                    return AggregationResult.Cancelled;
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }

    public (int minXPos, int minYPos, int maxXPos, int maxYPos)? GetAxisExtremesOf(IMap map)
    {
        return map.AcceptGeneric(this);
    }

    (int, int, int, int)? IMapGenericVisitor<(int, int, int, int)?>.GenericVisit<TMap>(TMap map)
    {
        foreach (var graphicsAggregator in MapGraphicsAggregators)
        {
            if (graphicsAggregator is IMapGraphicsAggregator<TMap> mapGraphicsAggregator)
            {
                return mapGraphicsAggregator.GetAxisExtremesOf(map);
            }
        }
        return null;
    }

    public AggregationResult AggregatePathGraphics(IPath path, IGraphicsObjectCollector collectorForAggregatedObjects)
    {
        return path.AcceptGeneric(this, collectorForAggregatedObjects);
    }

    AggregationResult IPathGenericVisitor<AggregationResult, IGraphicsObjectCollector>.GenericVisit<TPath>(TPath path, IGraphicsObjectCollector collectorForAggregatedObjects)
    {
        
        foreach (var graphicsAggregator in MapGraphicsAggregators)
        {
            if (graphicsAggregator is IPathGraphicsAggregator<TPath> pathGraphcisAggregator)
            {
                pathGraphcisAggregator.AggregateGraphics(path, collectorForAggregatedObjects);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }

    public List<AggregationResult> AggregatePathsGraphics(IPath[] paths,
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        List<AggregationResult> results = new List<AggregationResult>();
        foreach (var path in paths)
        {
            if(cancellationToken?.IsCancellationRequested ?? false) 
                results.Add(AggregationResult.Cancelled);
            else
                results.Add(AggregatePathGraphics(path, collectorForAggregatedObjects));
        }
        return results;
    }
}
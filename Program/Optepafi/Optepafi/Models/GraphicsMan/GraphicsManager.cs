using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.Graphics.GraphicsAggregators;
using Optepafi.Models.Graphics.GraphicsAggregators.MapGraphicsAggregators;
using Optepafi.Models.Graphics.GraphicsAggregators.Path;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.Models.Graphics;

/// <summary>
/// 
/// </summary>
public class GraphicsManager :
    IMapGenericVisitor<GraphicsManager.AggregationResult, (IGraphicsObjectCollector, CancellationToken?)>,
    IMapGenericVisitor<GraphicsArea?>,
    IMapGenericVisitor<GraphicsManager.AggregationResult, (IList<MapCoordinate>, IGraphicsObjectCollector)>
{
    public static GraphicsManager Instance { get; } = new();
    private GraphicsManager() { }

    public IReadOnlySet<IGraphicsAggregator> MapGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(TextMapGraphicsAggregator.Instance);

    
    public enum AggregationResult {Aggregated, NoUsableAggregatorFound, Cancelled}

    public AggregationResult AggregateMapGraphics(IMap map, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric<AggregationResult, (IGraphicsObjectCollector, CancellationToken?)>(this, (collectorForAggregatedObjects, cancellationToken));
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

    public GraphicsArea? GetAreaOf(IMap map)
    {
        return map.AcceptGeneric(this);
    }

    GraphicsArea? IMapGenericVisitor<GraphicsArea?>.GenericVisit<TMap>(TMap map)
    {
        foreach (var graphicsAggregator in MapGraphicsAggregators)
        {
            if (graphicsAggregator is IMapGraphicsAggregator<TMap> mapGraphicsAggregator)
            {
                return mapGraphicsAggregator.GetAreaOf(map);
            }
        }
        return null;
    }

    public AggregationResult AggregateTrackGraphicsAccordingTo(IList<MapCoordinate> trackCoordinates, IMap map, IGraphicsObjectCollector collectorForAggregateObjects)
    {
        return map.AcceptGeneric<AggregationResult, (IList<MapCoordinate>, IGraphicsObjectCollector)>(this, (trackCoordinates, collectorForAggregateObjects));
    }

    AggregationResult IMapGenericVisitor<AggregationResult, (IList<MapCoordinate>, IGraphicsObjectCollector)>.GenericVisit<TMap>(TMap map, (IList<MapCoordinate>, IGraphicsObjectCollector) otherParams)
    {
        var (trackCoordinates, collectorForAggregatedObjects) = otherParams;
        foreach (var graphicsAggregator in MapGraphicsAggregators)
        {
            if (graphicsAggregator is IMapGraphicsAggregator<TMap> mapGraphicsAggregator)
            {
                mapGraphicsAggregator.AggregateGraphicsOfTrack(trackCoordinates, collectorForAggregatedObjects);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }
}
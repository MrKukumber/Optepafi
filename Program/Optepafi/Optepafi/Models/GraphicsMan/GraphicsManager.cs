using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.GraphicsMan.Aggregators;
using Optepafi.Models.GraphicsMan.Aggregators.Map;
using Optepafi.Models.GraphicsMan.Aggregators.MapRepre.CompleteNetIntertwining;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan;

/// <summary>
/// Singleton class used for managing of graphics aggregation.
/// 
/// It is main channel between graphics aggregation mechanism and applications logic (ModelViews/ViewModels).  
/// It provides supporting methods for correct calling of graphics aggregators.  
/// Additionally there is also  <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> singleton which is intended to provide graphics aggregation services for other managers and constructs in Model.  
///
/// Map graphics aggregation is done by accepting <see cref="IGraphicObjectCollector"/> instance and filling it with graphic objects by appropriate aggregator.  
/// Reason for this design is possibility for application logic to retrieve graphics asynchronously so aggregated objects could be delivered right after their creation.  
/// Graphic objects are aggregated from provided construct, for which must aggregator exist. Its instance must be added to appropriate collection (either in this class or in sub-manager).  
/// </summary>
public class GraphicsManager :
    IMapGenericVisitor<GraphicsManager.AggregationResult, (IGraphicObjectCollector, CancellationToken?)>,
    IMapGenericVisitor<GraphicsArea?>,
    IMapGenericVisitor<GraphicsManager.AggregationResult, (IList<MapCoordinates>, IGraphicObjectCollector)>,
    IMapRepreGenericVisitor<GraphicsManager.AggregationResult, (IGraphicObjectCollector, CancellationToken?)>
{
    public static GraphicsManager Instance { get; } = new();
    private GraphicsManager() { }

    /// <summary>
    /// Collection of aggregators for specific map types. It is searched when map graphics is to be aggregated.
    /// </summary>
    public IReadOnlySet<IGraphicsAggregator> MapGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(TextMapGraphicsAggregator.Instance, OmapMapGraphicsAggregator.Instance);

    /// <summary>
    /// Collection of aggregators for specific map representation/graph implementation types. It is searched when map representation graphics is to be aggregated.
    /// </summary>
    public IReadOnlySet<IGraphicsAggregator> MapRepreGraphicsAggregators { get; } = 
        ImmutableHashSet.Create<IGraphicsAggregator>(
            CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator.Instance, 
            CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator.Instance,
            CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator.Instance);
    
    public enum AggregationResult {Aggregated, NoUsableAggregatorFound, Cancelled}

    /// <summary>
    /// Method for aggregating of map graphics.
    /// 
    /// It accepts map for aggregation and collector which will be filled with aggregated objects.  
    /// It do it so by use of "generic visitor pattern" on map. After visiting of map it runs through <c>MapGraphicsAggregators</c> and looks for appropriate graphics aggregator.  
    /// When such aggregator is found, its aggregating method is called.  
    /// </summary>
    /// <param name="map">Map whose graphics is to be aggregated.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated map graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancellation of aggregation.</param>
    /// <returns>Result of aggregation.</returns>
    public AggregationResult AggregateMapGraphics(IMap map, IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
    {
        return map.AcceptGeneric<AggregationResult, (IGraphicObjectCollector, CancellationToken?)>(this, (collectorForAggregatedObjects, cancellationToken));
    }
    
    AggregationResult IMapGenericVisitor<AggregationResult, (IGraphicObjectCollector, CancellationToken?)>.GenericVisit<TMap>(TMap map, (IGraphicObjectCollector, CancellationToken?) otherParams)
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

    /// <summary>
    /// Method for retrieving the area of specific map.
    /// 
    /// Area of map is meant to be rectangle which contains whole map. It is defined by its left-bottom and right-top vertices.  
    /// Retrieved area then can be used for creating of <see cref="IGroundGraphicsSource"/> tied to this map.  
    /// It uses "generic visitor pattern" on provided map. After visiting of map it runs through <c>MapGraphicsAggregators</c> and looks for appropriate graphics aggregator.  
    /// When such aggregator is found, method for retrieving are of map is called on it. Area is then returned.  
    /// </summary>
    /// <param name="map">Map whose area is requested.</param>
    /// <returns>Area of map. Null, when aggregator is not found.</returns>
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

    /// <summary>
    /// Method for aggregating graphics of provided track.
    /// 
    /// It accepts track to be aggregated, map for indication of correct aggregator and collector which should be filled with aggregated objects.  
    /// It do it so by use of "generic visitor pattern" on provided map.  After visiting of map it runs through <c>MapGraphicsAggregators</c> and looks for appropriate graphics aggregator.  
    /// When it is found, the track coordinates are handed over to it together with graphics objects collector.  
    /// </summary>
    /// <param name="trackCoordinates">Coordinates of track which graphics is to be aggregated.</param>
    /// <param name="map">Map for indication of correct aggregator.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated track graphic objects.</param>
    /// <returns>Result of aggregation.</returns>
    public AggregationResult AggregateTrackGraphicsAccordingTo(IList<MapCoordinates> trackCoordinates, IMap map, IGraphicObjectCollector collectorForAggregatedObjects)
    {
        return map.AcceptGeneric<AggregationResult, (IList<MapCoordinates>, IGraphicObjectCollector)>(this, (trackCoordinates, collectorForAggregatedObjects));
    }

    AggregationResult IMapGenericVisitor<AggregationResult, (IList<MapCoordinates>, IGraphicObjectCollector)>.GenericVisit<TMap>(TMap map, (IList<MapCoordinates>, IGraphicObjectCollector) otherParams)
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

    //TODO: comment
    public AggregationResult AggregateMapRepreGraphics(IMapRepre mapRepre,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
    {
        return mapRepre.AcceptGeneric(this, (collectorForAggregatedObjects, cancellationToken));
    }

    AggregationResult IMapRepreGenericVisitor<AggregationResult, (IGraphicObjectCollector, CancellationToken?)>.GenericVisit<TImplementation>(TImplementation implementation,
        (IGraphicObjectCollector, CancellationToken?) otherParams) 
    {
        var (collectorForAggregatedObjects, cancellationToken) = otherParams;
        foreach (var graphicsAggregator in MapRepreGraphicsAggregators)
        {
            if (graphicsAggregator is IMapRepreGraphicsAggregator<TImplementation> implementationGraphcisAggregator)
            {
                lock (implementation)
                {
                    implementationGraphcisAggregator.AggregateGraphics(implementation, collectorForAggregatedObjects, cancellationToken);
                }
                if (cancellationToken?.IsCancellationRequested ?? false)
                    return AggregationResult.Cancelled;
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }
}
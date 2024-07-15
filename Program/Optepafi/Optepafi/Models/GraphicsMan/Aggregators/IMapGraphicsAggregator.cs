using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.GraphicsMan.Aggregators;

/// <summary>
/// Represents aggregator of graphic objects for maps of specific type.
/// 
/// It provides graphics aggregation for maps themselves, retrieval of their area and graphics aggregation for tracks in style of represented map type.  
/// Graphics is aggregated into provided collectors. This aggregation will probably run asynchronously, so aggregated objects could be used right after their submission to collector.  
/// </summary>
/// <typeparam name="TMap">Type of map for which is graphics aggregated.</typeparam>
public interface IMapGraphicsAggregator<in TMap> : IGraphicsAggregator
    where TMap : IMap
{
    /// <summary>
    /// Aggregates graphics of provided map into provided collector.
    ///
    /// Aggregation will probably run asynchronously, so aggregated objects could be used right after their submission.  
    /// Because of that, objects should be submitted into collector immediately after their creation, so the app logic could process them.  
    /// </summary>
    /// <param name="map">Map on which aggregation of graphic objects is based.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling of aggregation.</param>
    public void AggregateGraphics(TMap map, IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
    
    /// <summary>
    /// Method for retrieving area of provided map.
    /// 
    /// Area is a rectangle that encapsulates whole map and therefore maps graphics too.  
    /// </summary>
    /// <param name="map">Map which area should be retrieved.</param>
    /// <returns>Area encapsulating inputted map.</returns>
    public GraphicsArea GetAreaOf(TMap map);
    
    /// <summary>
    /// Aggregates graphics of provided track according to style of represented map type. Aggregated objects are submitted into provided collector.
    /// 
    /// Aggregation will probably run asynchronously, so aggregated object could be used right after their submission.  
    /// Because of that, objects should be submitted into collector immediately after their creation, so the app logic could process them.   
    /// </summary>
    /// <param name="track">Track whose graphic representation is generated.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated graphic objects.</param>
    public void AggregateGraphicsOfTrack(IList<MapCoordinate> track, IGraphicObjectCollector collectorForAggregatedObjects);
}
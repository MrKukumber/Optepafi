using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.GraphicsMan.Aggregators.Map;

/// <summary>
/// Singleton class representing aggregator of graphic objects for <see cref="TextMap"/> map type.
/// For more information on map graphics aggregators see <see cref="IMapGraphicsAggregator{TMap}"/>.
/// </summary>
public class TextMapGraphicsAggregator : IMapGraphicsAggregator<TextMap>
{
    public static TextMapGraphicsAggregator Instance { get; } = new();
    private TextMapGraphicsAggregator(){}
    
    /// <inheritdoc cref="IMapGraphicsAggregator{TMap}.AggregateGraphics"/>
    /// <remarks>
    /// <c>TextMap</c> contains text of some text file.
    /// This text is split into words and then each word is assignet to <c>WordObject</c> with random position in range -50000 to 50000 micrometers.
    /// </remarks>
    public void AggregateGraphics(TextMap map, IGraphicObjectCollector collectorForAggregatedObjects,
        CancellationToken? cancellationToken)
    {
        string[] words = map.Text.Split();
        Random generator = new Random();
        foreach (var word in words)
        {
            collectorForAggregatedObjects.Add(new WordObject(
                new MapCoordinate(
                    generator.Next(-50000,50000), 
                    generator.Next(-50000,50000)), 
                word));
            if (cancellationToken?.IsCancellationRequested ?? false) return;
        }
    }

    /// <inheritdoc cref="IMapGraphicsAggregator{TMap}.AggregateGraphicsOfTrack"/>
    /// <remarks>
    /// For each point of track returns <c>TrackPointWordObject</c> with its position.
    /// </remarks>
    public void AggregateGraphicsOfTrack(IList<MapCoordinate> track, IGraphicObjectCollector collectorForAggregatedObjects)
    {
        List<TrackPointWordObject> trackPointWordObjects = new();
        foreach (var trackCoordinate in track)
        {
            trackPointWordObjects.Add(new TrackPointWordObject(trackCoordinate));
        }
        collectorForAggregatedObjects.AddRange(trackPointWordObjects);
    }

    /// <inheritdoc cref="IMapGraphicsAggregator{TMap}.GetAreaOf"/>
    /// <remarks>
    /// Area of each TextMap is set be of range -50000 to 50000 micrometers both horizontally and vertically.
    /// </remarks>
    public GraphicsArea GetAreaOf(TextMap map)
    {
        return new GraphicsArea(new MapCoordinate(-50000, -50000), new MapCoordinate(50000, 50000));
    }
}
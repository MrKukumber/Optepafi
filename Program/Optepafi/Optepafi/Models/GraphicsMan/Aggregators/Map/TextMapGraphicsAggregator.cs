using System;
using System.Collections.Generic;
using System.Threading;
using DynamicData;
using Optepafi.Models.Graphics.GraphicsObjects.MapObjects;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.Graphics.GraphicsAggregators.MapGraphicsAggregators;

public class TextMapGraphicsAggregator : IMapGraphicsAggregator<TextMap>
{
    public static TextMapGraphicsAggregator Instance { get; } = new();
    private TextMapGraphicsAggregator(){}
    public IMapIdentifier<TextMap> UsedMapIdentifier => TextMapRepresentative.Instance;
    public void AggregateGraphics(TextMap map, IGraphicsObjectCollector collectorForAggregatedObjects,
        CancellationToken? cancellationToken)
    {
        string[] words = map.Text.Split();
        Random generator = new Random();
        foreach (var word in words)
        {
            collectorForAggregatedObjects.Add(new WordObject(
                new MapCoordinate(
                    generator.Next(0,100000), 
                    generator.Next(0,100000)), 
                word));
            if (cancellationToken?.IsCancellationRequested ?? false) return;
        }
    }

    public void AggregateGraphicsOfTrack(IList<MapCoordinate> track, IGraphicsObjectCollector collectorForAggregatedObjects)
    {
        List<TrackPointWordObject> trackPointWordObjects = new();
        foreach (var trackCoordinate in track)
        {
            trackPointWordObjects.Add(new TrackPointWordObject(trackCoordinate));
        }
        collectorForAggregatedObjects.AddRange(trackPointWordObjects);
    }

    public GraphicsArea GetAreaOf(TextMap map)
    {
        return new GraphicsArea(new MapCoordinate(0, 0), new MapCoordinate(100000, 100000));
    }
}
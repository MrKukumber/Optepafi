using System;
using System.Threading;
using Optepafi.Models.Graphics.GraphicsObjects.MapObjects;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.Graphics.GraphicsAggregators.MapGraphicsAggregators;

public class TextMapGraphicsAggregator : IMapGraphicsAggregator<TextMap>
{
    public static TextMapGraphicsAggregator Instance { get; } = new();
    private TextMapGraphicsAggregator(){}
    
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

    public GraphicsArea GetAreaOf(TextMap map)
    {
        return new GraphicsArea(new MapCoordinate(0, 0), new MapCoordinate(100000, 100000));
    }
}
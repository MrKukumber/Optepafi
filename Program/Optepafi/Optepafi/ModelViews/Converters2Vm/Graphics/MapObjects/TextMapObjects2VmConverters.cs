using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.GraphicsObjects.MapObjects;
using Optepafi.Models.MapMan;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.MapObjects;

namespace Optepafi.ModelViews.Converters.Graphics.MapObjects;

public static class TextMapObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        [typeof(WordObject)] =  WordObject2VmConverter.Instance,
        [typeof(TrackPointWordObject)] = TrackPointWordObject2VmConverter.Instance
    };
}

public class WordObject2VmConverter : IGraphicObjects2VmConverter<WordObject>
{
    public static WordObject2VmConverter Instance { get; } = new();
    private WordObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(WordObject wordObject, MapCoordinate mapsLeftBottomVertex)
    {
        return new WordObjectViewModel(wordObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), wordObject.Text);
    }
}

public class TrackPointWordObject2VmConverter : IGraphicObjects2VmConverter<TrackPointWordObject>
{
    
    public static TrackPointWordObject2VmConverter Instance { get; } = new();
    private TrackPointWordObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(TrackPointWordObject trackPointWordObject, MapCoordinate mapsLefBottomVertex)
    {
        return new TrackPointWordObjectViewModel(trackPointWordObject.Position.ToCanvasCoordinate(mapsLefBottomVertex));
    }
}

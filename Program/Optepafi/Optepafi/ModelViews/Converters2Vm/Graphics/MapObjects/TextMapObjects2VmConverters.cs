using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.MapObjects;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapObjects;

/// <summary>
/// Static class which contains dictionary of <c>TextMap</c> graphic object to ViewModel converters.
/// 
/// It should contain all such converters. It is concatenated to dictionary of all other map graphic object converters in <see cref="MapObjects2VmConverters"/>.  
/// </summary>
public static class TextMapObjects2VmConverters
{
    
    /// <summary>
    /// Dictionary of text map graphic object converters. 
    /// </summary>
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        [typeof(WordObject)] =  WordObject2VmConverter.Instance,
        [typeof(TrackPointWordObject)] = TrackPointWordObject2VmConverter.Instance
    };
}

/// <summary>
/// Converter of word graphic object to ViewModel.
///
/// It is included in dictionary of text map graphic object to ViewModel converters in <see cref="TextMapObjects2VmConverters"/>. 
/// </summary>
public class WordObject2VmConverter : IGraphicObjects2VmConverter<WordObject>
{
    public static WordObject2VmConverter Instance { get; } = new();
    private WordObject2VmConverter(){}
    
    /// <inheritdoc cref="IGraphicObjects2VmConverter{TGraphicsObject}.ConvertToViewModel"/>
    public GraphicObjectViewModel ConvertToViewModel(WordObject wordObject, MapCoordinate mapsLeftBottomVertex)
    {
        return new WordObjectViewModel(wordObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), wordObject.Text);
    }
}


/// <summary>
/// Converter of track point graphic object of <c>TextMap</c> design to ViewModel.
///
/// It is included in dictionary of text map graphic object to ViewModel converters in <see cref="TextMapObjects2VmConverters"/>. 
/// </summary>
public class TrackPointWordObject2VmConverter : IGraphicObjects2VmConverter<TrackPointWordObject>
{
    
    public static TrackPointWordObject2VmConverter Instance { get; } = new();
    private TrackPointWordObject2VmConverter(){}
    
    /// <inheritdoc cref="IGraphicObjects2VmConverter{TGraphicsObject}.ConvertToViewModel"/>
    public GraphicObjectViewModel ConvertToViewModel(TrackPointWordObject trackPointWordObject, MapCoordinate mapsLefBottomVertex)
    {
        return new TrackPointWordObjectViewModel(trackPointWordObject.Position.ToCanvasCoordinate(mapsLefBottomVertex));
    }
}

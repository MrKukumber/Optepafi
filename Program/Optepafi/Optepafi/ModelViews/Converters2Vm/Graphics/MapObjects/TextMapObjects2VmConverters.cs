using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.GraphicsObjects.MapObjects;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.MapObjects;

namespace Optepafi.ModelViews.Converters.Graphics.MapObjects;

public static class TextMapObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        [typeof(WordObject)] =  WordObject2VmConverter.Instance,
    };
}

public class WordObject2VmConverter : IGraphicObjects2VmConverter<WordObject>
{
    public static WordObject2VmConverter Instance { get; } = new();
    private WordObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(WordObject wordObject, int minimalXPosition, int minimalYPosition)
    {
        return new WordObjectViewModel(wordObject.Text, wordObject.Position.XPos - minimalXPosition,
            wordObject.Position.YPos - minimalYPosition);
    }
}
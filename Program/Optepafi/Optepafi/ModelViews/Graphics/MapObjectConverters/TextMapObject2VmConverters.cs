using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.GraphicsObjects.MapObjects;
using Optepafi.ViewModels;
using Optepafi.ViewModels.Graphics;
using Optepafi.ViewModels.Graphics.MapObjects;

namespace Optepafi.ModelViews.Graphics.MapObjectConverters;

public static class TextMapObject2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VMConverter> AllConverters = new()
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
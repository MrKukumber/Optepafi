using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects.Path;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.PathObjects;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.PathObjects;

//TODO: comment
public class SegmentedLinePathObjects2VmConverters
{

    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        { typeof(SegmentedLineObject), SegmentedLineObject2VmConvertor.Instance }
    };
}

//TODO: comment
public class SegmentedLineObject2VmConvertor : IGraphicObjects2VmConverter<SegmentedLineObject>
{
    public static SegmentedLineObject2VmConvertor Instance = new();
    private SegmentedLineObject2VmConvertor() { }
    public GraphicObjectViewModel ConvertToViewModel(SegmentedLineObject graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new SegmentedLineObjectViewModel(graphicsObject, mapsTopLeftVertex);
}
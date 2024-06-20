using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.MapMan;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.PathObjects;

namespace Optepafi.ModelViews.Converters.Graphics.PathObjects;

public class SmileyFacePathObjects2VmConverters
{
    
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        // [typeof(SmileyFaceObject)] = SmileyFaceObject2VmConverter.Instance
        [typeof(SmileyFaceEyeObject)] = SmileyFaceEyeObject2VmConverter.Instance,
        [typeof(SmileyFaceNoseObject)] = SmileyFaceNoseObject2VmConverter.Instance,
        [typeof(SmileyFaceMouthObject)] = SmileyFaceMouthObject2VmConverter.Instance,
    };
}

public class SmileyFaceEyeObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceEyeObject>
{
    public static SmileyFaceEyeObject2VmConverter Instance { get; } = new();
    private SmileyFaceEyeObject2VmConverter(){}

    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceEyeObject graphicsObject, MapCoordinate mapsLeftBottomVertex)
    {
        return new SmileyFaceEyeObjectViewModel(graphicsObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), graphicsObject.Width, graphicsObject.Height);
    }
}

public class SmileyFaceNoseObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceNoseObject>
{
    public static SmileyFaceNoseObject2VmConverter Instance { get; } = new();
    private SmileyFaceNoseObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceNoseObject graphicsObject, MapCoordinate mapsLeftBottomVertex)
    {
        
        return new SmileyFaceNoseObjectViewModel(graphicsObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), graphicsObject.Width, graphicsObject.Height);
    }
}

public class SmileyFaceMouthObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceMouthObject>
{
    public static SmileyFaceMouthObject2VmConverter Instance { get; } = new();
    private SmileyFaceMouthObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceMouthObject graphicsObject, MapCoordinate mapsLeftBottomVertex)
    {
        return new SmileyFaceMouthObjectViewModel(
            graphicsObject.BezierCurveData.Item1.ToCanvasCoordinate(mapsLeftBottomVertex), (
                new MapCoordinate(0, 0),
                graphicsObject.BezierCurveData.Item2 - graphicsObject.BezierCurveData.Item1,
                graphicsObject.BezierCurveData.Item3 - graphicsObject.BezierCurveData.Item1,
                graphicsObject.BezierCurveData.Item4 - graphicsObject.BezierCurveData.Item1));
    }
}

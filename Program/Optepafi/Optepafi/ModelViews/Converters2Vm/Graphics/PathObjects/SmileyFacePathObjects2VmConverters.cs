using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.MapMan;
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

    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceEyeObject graphicsObject, int minimalXPosition,
        int minimalYPosition)
    {
        return new SmileyFaceEyeObjectViewModel(graphicsObject.Position.XPos - minimalXPosition,
            graphicsObject.Position.YPos - minimalYPosition, graphicsObject.Width, graphicsObject.Height);
    }
}

public class SmileyFaceNoseObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceNoseObject>
{
    public static SmileyFaceNoseObject2VmConverter Instance { get; } = new();
    private SmileyFaceNoseObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceNoseObject graphicsObject, int minimalXPosition,
        int minimalYPosition)
    {
        
        return new SmileyFaceNoseObjectViewModel(graphicsObject.Position.XPos - minimalXPosition,
            graphicsObject.Position.YPos - minimalYPosition, graphicsObject.Width, graphicsObject.Height);
    }
}

public class SmileyFaceMouthObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceMouthObject>
{
    public static SmileyFaceMouthObject2VmConverter Instance { get; } = new();
    private SmileyFaceMouthObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceMouthObject graphicsObject, int minimalXPosition,
        int minimalYPosition)
    {
        return new SmileyFaceMouthObjectViewModel(
            graphicsObject.BezierCurveData.Item1.XPos - minimalXPosition,
            graphicsObject.BezierCurveData.Item1.YPos - minimalYPosition, (
                new MapCoordinate(0, 0),
                graphicsObject.BezierCurveData.Item2 - graphicsObject.BezierCurveData.Item1,
                graphicsObject.BezierCurveData.Item3 - graphicsObject.BezierCurveData.Item1,
                graphicsObject.BezierCurveData.Item4 - graphicsObject.BezierCurveData.Item1));
    }
}

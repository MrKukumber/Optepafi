using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects.Path;
using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.PathObjects;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.PathObjects;


/// <summary>
/// Static class which contains dictionary of <c>SmileyFacePath{TVertexAttributes,TEdgeAttributes}</c> graphic object to ViewModel converters.
/// 
/// It should contain all such converters. It is concatenated to dictionary of all other path graphic object converters in <see cref="PathObjects2VmConverters"/>.  
/// </summary>
public static class SmileyFacePathObjects2VmConverters
{
    
    /// <summary>
    /// Dictionary of smiley face path graphic object converters. 
    /// </summary>
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        [typeof(SmileyFaceEyeObject)] = SmileyFaceEyeObject2VmConverter.Instance,
        [typeof(SmileyFaceNoseObject)] = SmileyFaceNoseObject2VmConverter.Instance,
        [typeof(SmileyFaceMouthObject)] = SmileyFaceMouthObject2VmConverter.Instance,
    };
}

/// <summary>
/// Converter of smiley faces eye graphic object to ViewModel.
///
/// It is included in dictionary of smiley face path graphic object to ViewModel converters in <see cref="SmileyFacePathObjects2VmConverters"/>.  
/// </summary>
public class SmileyFaceEyeObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceEyeObject>
{
    public static SmileyFaceEyeObject2VmConverter Instance { get; } = new();
    private SmileyFaceEyeObject2VmConverter(){}

    /// <inheritdoc cref="IGraphicObjects2VmConverter{TGraphicsObject}.ConvertToViewModel"/>.
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceEyeObject graphicsObject, MapCoordinates mapsLeftBottomVertex)
    {
        return new SmileyFaceEyeObjectViewModel(graphicsObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), graphicsObject.Width, graphicsObject.Height);
    }
}

/// <summary>
/// Converter of smiley faces nose graphic object to ViewModel.
///
/// It is included in dictionary of smiley face path graphic object to ViewModel converters in <see cref="SmileyFacePathObjects2VmConverters"/>.  
/// </summary>
public class SmileyFaceNoseObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceNoseObject>
{
    public static SmileyFaceNoseObject2VmConverter Instance { get; } = new();
    private SmileyFaceNoseObject2VmConverter(){}
    
    
    /// <inheritdoc cref="IGraphicObjects2VmConverter{TGraphicsObject}.ConvertToViewModel"/>.
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceNoseObject graphicsObject, MapCoordinates mapsLeftBottomVertex)
    {
        return new SmileyFaceNoseObjectViewModel(graphicsObject.Position.ToCanvasCoordinate(mapsLeftBottomVertex), graphicsObject.Width, graphicsObject.Height);
    }
}

/// <summary>
/// Converter of smiley faces mouth graphic object to ViewModel.
///
/// It is included in dictionary of smiley face path graphic object to ViewModel converters in <see cref="SmileyFacePathObjects2VmConverters"/>.  
/// </summary>
public class SmileyFaceMouthObject2VmConverter : IGraphicObjects2VmConverter<SmileyFaceMouthObject>
{
    public static SmileyFaceMouthObject2VmConverter Instance { get; } = new();
    private SmileyFaceMouthObject2VmConverter(){}
    
    /// <inheritdoc cref="IGraphicObjects2VmConverter{TGraphicsObject}.ConvertToViewModel"/>.
    /// <remarks>
    /// Position of created ViewModel is set to be the first coordinate of bezier curve. All coordinates of bezier curve are positioned accordingly to this first coordinate.  
    /// </remarks>
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceMouthObject graphicsObject, MapCoordinates mapsLeftBottomVertex)
    {
        CanvasCoordinate position = graphicsObject.BezierCurveData.Item1.ToCanvasCoordinate(mapsLeftBottomVertex);
        return new SmileyFaceMouthObjectViewModel(
            position, (
                new CanvasCoordinate(0, 0),
                graphicsObject.BezierCurveData.Item2.ToCanvasCoordinate(mapsLeftBottomVertex) - position,
                graphicsObject.BezierCurveData.Item3.ToCanvasCoordinate(mapsLeftBottomVertex) - position,
                graphicsObject.BezierCurveData.Item4.ToCanvasCoordinate(mapsLeftBottomVertex) - position));
    }
}

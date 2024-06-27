using Optepafi.Models.MapMan;
using Optepafi.ViewModels.DataViewModels;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Graphics.PathObjects;

/// <summary>
/// ViewModel for <c>SmileyFaceEyeObject</c> graphic object type.
/// There should exists appropriate convertor of <c>SmileyFaceEyeObject</c> to this ViewModel type.
/// For more information on graphic object ViewModels see <see cref="GraphicObjectViewModel"/>.
/// </summary>
public class SmileyFaceEyeObjectViewModel : GraphicObjectViewModel
{
    /// <summary>
    /// Creates instance of this type from provided parameters.
    /// Position of eye object is adjusted little bit so it was centered on the provided position.  
    /// Priority is chosen very specifically for smiley face path objects.
    /// </summary>
    /// <param name="position">Position of eye object.</param>
    /// <param name="width">Width of eye object.</param>
    /// <param name="height">Height of eye object.</param>
    public SmileyFaceEyeObjectViewModel(CanvasCoordinate position, int width, int height)
    {
        Position = new CanvasCoordinate(position.LeftPos - width/2, position.BottomPos - height/2);
        Priority = 420;
        Width = width;
        Height = height;
    }
    /// <inheritdoc cref="GraphicObjectViewModel.Position"/>
    public override CanvasCoordinate Position { get; }
    /// <inheritdoc cref="GraphicObjectViewModel.Priority"/>
    public override int Priority { get; }
    /// <summary>
    /// Width of an eye object.
    /// </summary>
    public int Width { get; }
    /// <summary>
    /// Height of an eye object.
    /// </summary>
    public int Height { get; }
}

/// <summary>
/// ViewModel for <c>SmileyFaceNoseObject</c> graphic object type.
/// There should exists appropriate convertor of <c>SmileyFaceNoseObject</c> to this ViewModel type.
/// For more information on graphic object ViewModels see <see cref="GraphicObjectViewModel"/>.
/// </summary>
public class SmileyFaceNoseObjectViewModel : GraphicObjectViewModel
{
    /// <summary>
    /// Creates instance of this type from provided parameters.
    /// Position of nose object is adjusted little bit so it was centered on the provided position.  
    /// Priority is chosen very specifically for smiley face path objects.
    /// </summary>
    /// <param name="position">Position of nose object.</param>
    /// <param name="width">Width of nose object.</param>
    /// <param name="height">Height of nose object.</param>
    public SmileyFaceNoseObjectViewModel(CanvasCoordinate position, int width, int height)
    {
        Position = new CanvasCoordinate(position.LeftPos - width/2, position.BottomPos - height/2);
        Priority = 420;
        Width = width;
        Height = height;
    }
    
    /// <inheritdoc cref="GraphicObjectViewModel.Position"/>
    public override CanvasCoordinate Position { get; }
    /// <inheritdoc cref="GraphicObjectViewModel.Priority"/>
    public override int Priority { get; }
    /// <summary>
    /// Width of an nose object.
    /// </summary>
    public int Width { get; }
    /// <summary>
    /// Height of an nose object.
    /// </summary>
    public int Height { get; }
}

/// <summary>
/// ViewModel for <c>SmileyFaceMouthObject</c> graphic object type.
/// There should exists appropriate convertor of <c>SmileyFaceMouthObject</c> to this ViewModel type.
/// For more information on graphic object ViewModels see <see cref="GraphicObjectViewModel"/>.
/// </summary>
public class SmileyFaceMouthObjectViewModel : GraphicObjectViewModel
{
    
    /// <summary>
    /// Creates an instance of this type from provided parameters.
    /// Priority is chosen very specifically for smiley face path objects.
    /// Bezier curve data are positioned relatively to provided position.
    /// </summary>
    /// <param name="position">Position of mouth object.</param>
    /// <param name="bezierCurveData">Positions of points which define bezier curve which draw the mouth.</param>
    public SmileyFaceMouthObjectViewModel(CanvasCoordinate position, (CanvasCoordinate, CanvasCoordinate, CanvasCoordinate, CanvasCoordinate) bezierCurveData)
    {
        Position = position;
        Priority = 420;
        StartPoint = bezierCurveData.Item1;
        Point2 = bezierCurveData.Item2;
        Point3 = bezierCurveData.Item3;
        Point4 = bezierCurveData.Item4;
    }
    
    /// <inheritdoc cref="GraphicObjectViewModel.Position"/>
    public override CanvasCoordinate Position { get; }
    /// <inheritdoc cref="GraphicObjectViewModel.Priority"/>
    public override int Priority { get; }
    /// <summary>
    /// Starting point of bezier curve which draws the mouth.
    /// </summary>
    public CanvasCoordinate StartPoint { get; }
    /// <summary>
    /// Second point defining bezier curve which draws the mouth.
    /// </summary>
    public CanvasCoordinate Point2 { get; }
    /// <summary>
    /// Third point defining bezier curve which draws the mouth.
    /// </summary>
    public CanvasCoordinate Point3 { get; }
    /// <summary>
    /// Fourth point defining bezier curve which draws the mouth.
    /// </summary>
    public CanvasCoordinate Point4 { get; }
}

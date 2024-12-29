using Optepafi.Models.GraphicsMan.Objects.Path;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Shapes;

namespace Optepafi.ViewModels.Data.Graphics.PathObjects;

//TODO: comment
public class SegmentedLineObjectViewModel(SegmentedLineObject obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Path.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 100;
    public PathPolygonViewModel Shape { get; } = new (obj.Path, mapsTopLeftVertex);
    public int LineThickness { get; } =  250;
    public float Opacity { get; } = 0.8f;
}
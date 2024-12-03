using Optepafi.Models.GraphicsMan.Objects.MapRepre.CompleteNetIntertwiningMapRepre;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Utils;

namespace Optepafi.ViewModels.Data.Graphics.MapRepreObjects.CompleteNetIntertwiningMapRepre;

public class VertexObjectViewModel(VertexObject obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Position.ToCanvasCoordinate(mapsTopLeftVertex) - new CanvasCoordinate(500, 500);
    public override int Priority { get; } = 36;
    public int Diameter { get; } = 1000;
}
public class EdgeObjectViewModel(EdgeObject obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.From.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority { get; } = 35;

    public CanvasCoordinate StartPoint { get; } = new CanvasCoordinate(0, 0);
    
    public CanvasCoordinate EndPoint { get; } = obj.To.ToCanvasCoordinate(mapsTopLeftVertex) - obj.From.ToCanvasCoordinate(mapsTopLeftVertex);
    public int LineThickness { get; } = 150;
}

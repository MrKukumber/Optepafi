using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class CubicBezierCurveSegmentViewModel(CubicBezierCurveSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsLeftBottomVertex) : SegmentViewModel
{
    public CanvasCoordinate Point1 { get; }=  segment.Point1.ToCanvasCoordinate(mapsLeftBottomVertex) - relativePosition;
    public CanvasCoordinate Point2 { get; }= segment.Point2.ToCanvasCoordinate(mapsLeftBottomVertex) - relativePosition;
    public CanvasCoordinate Point3 { get; }= segment.Point3.ToCanvasCoordinate(mapsLeftBottomVertex) - relativePosition;
}
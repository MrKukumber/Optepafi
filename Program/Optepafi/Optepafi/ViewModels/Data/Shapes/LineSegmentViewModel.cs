using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class LineSegmentViewModel(LineSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsLeftBottomVertex) : SegmentViewModel
{
    public CanvasCoordinate Point1 { get; } = segment.Point1.ToCanvasCoordinate(mapsLeftBottomVertex) - relativePosition;
}
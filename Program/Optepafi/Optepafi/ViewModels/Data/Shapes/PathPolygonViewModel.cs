using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class PathPolygonViewModel : 
    DataViewModel, 
    ISegmentVisitor<SegmentViewModel, MapCoordinates>
{
    public PathPolygonViewModel(Path path, MapCoordinates mapsLeftBottomVertex)
    {
        Position = path.StartPoint.ToCanvasCoordinate(mapsLeftBottomVertex);
        SegmentsRelativeToPosition = path.Segments.Select(segment => segment.Accept(this, mapsLeftBottomVertex));
    }
    public PathPolygonViewModel(Polygon polygon, MapCoordinates mapsLeftBottomVertex)
    {
        Position = polygon.Segments.Last().LastPoint.ToCanvasCoordinate(mapsLeftBottomVertex);
        SegmentsRelativeToPosition = polygon.Segments.Select(segment => segment.Accept(this, mapsLeftBottomVertex));
    }
    SegmentViewModel ISegmentVisitor<SegmentViewModel, MapCoordinates>.GenericVisit(CubicBezierCurveSegment segment, MapCoordinates otherParams) =>
        new CubicBezierCurveSegmentViewModel(segment, Position, otherParams);
    SegmentViewModel ISegmentVisitor<SegmentViewModel, MapCoordinates>.GenericVisit(LineSegment segment, MapCoordinates otherParams) =>
        new LineSegmentViewModel(segment, Position, otherParams);
    
    public CanvasCoordinate Position { get; } 
    public IEnumerable<SegmentViewModel> SegmentsRelativeToPosition { get; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Media;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;
using LineSegment = Optepafi.Models.Utils.Shapes.LineSegment;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class PathPolygonViewModel : 
    DataViewModel, 
    ISegmentVisitor<SegmentViewModel, MapCoordinates>
{
    public PathPolygonViewModel(Path path, MapCoordinates mapsTopLeftVertex)
    {
        Position = path.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
        SegmentsRelativeToPosition = path.Segments.Select(segment => segment.Accept(this, mapsTopLeftVertex));
    }
    public PathPolygonViewModel(Polygon polygon, MapCoordinates mapsTopLeftVertex)
    {
        Position = polygon.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
        SegmentsRelativeToPosition = polygon.Segments.Select(segment => segment.Accept(this, mapsTopLeftVertex));
    }
    SegmentViewModel ISegmentVisitor<SegmentViewModel, MapCoordinates>.GenericVisit(CubicBezierCurveSegment segment, MapCoordinates otherParams) =>
        new CubicBezierCurveSegmentViewModel(segment, Position, otherParams);
    SegmentViewModel ISegmentVisitor<SegmentViewModel, MapCoordinates>.GenericVisit(LineSegment segment, MapCoordinates otherParams) =>
        new LineSegmentViewModel(segment, Position, otherParams);
    
    public CanvasCoordinate Position { get; } 
    public IEnumerable<SegmentViewModel> SegmentsRelativeToPosition { get; }
    public string Data
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("M 0,0 ");
            foreach (var segmet in SegmentsRelativeToPosition)
                sb.Append(segmet.GetStringRep());
            return sb.ToString();
        }
    }
}
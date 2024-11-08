using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.Models.Utils.Shapes.Segments;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;
using LineSegment = Optepafi.Models.Utils.Shapes.Segments.LineSegment;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class PathPolygonViewModel : 
    DataViewModel, 
    ISegmentVisitor<SegmentViewModel, (CanvasCoordinate, MapCoordinates)>
{
    public PathPolygonViewModel(Path path, MapCoordinates mapsTopLeftVertex)
    {
        if (path.StartPoint.XPos == 213590)
        {
            Console.WriteLine();
        }
        StartPoint = new CanvasCoordinate(0, 0);     
        Position = path.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
        Segments = path.Segments.Select(segment => segment.Accept(this, (Position, mapsTopLeftVertex)));
    }
    public PathPolygonViewModel(Polygon polygon, MapCoordinates mapsTopLeftVertex)
    {
        StartPoint = new CanvasCoordinate(0, 0);     
        Position = polygon.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
        Segments = polygon.Segments.Select(segment => segment.Accept(this, (Position, mapsTopLeftVertex)));
    }
    SegmentViewModel ISegmentVisitor<SegmentViewModel, (CanvasCoordinate, MapCoordinates)>.GenericVisit(CubicBezierCurveSegment segment, (CanvasCoordinate , MapCoordinates ) otherParams) =>
        new CubicBezierCurveSegmentViewModel(segment, otherParams.Item1, otherParams.Item2);
    SegmentViewModel ISegmentVisitor<SegmentViewModel, (CanvasCoordinate, MapCoordinates)>.GenericVisit(QuadraticBezierCurveSegment segment, (CanvasCoordinate , MapCoordinates) otherParams) =>
        new QuadraticBezierCurveSegmentViewModel(segment, otherParams.Item1, otherParams.Item2);
    SegmentViewModel ISegmentVisitor<SegmentViewModel, (CanvasCoordinate, MapCoordinates)>.GenericVisit(LineSegment segment, (CanvasCoordinate , MapCoordinates) otherParams) =>
        new LineSegmentViewModel(segment, otherParams.Item1, otherParams.Item2);
    
    public CanvasCoordinate Position { get; } 
    public CanvasCoordinate StartPoint { get; }
    public IEnumerable<SegmentViewModel> Segments { get; }
    public string Data
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("M " + MicrometersToDipConverter.Instance.Convert(StartPoint.LeftPos).ToString(CultureInfo.InvariantCulture) 
                           + "," + MicrometersToDipConverter.Instance.Convert(StartPoint.LeftPos).ToString(CultureInfo.InvariantCulture) + " ");
            foreach (var segmet in Segments)
                sb.Append(segmet.GetStringRep());
            return sb.ToString();
        }
    }
    

    public PathPolygonViewModel GetTopAlignmentWithRespectTo(int thickness)
    {
        CanvasCoordinate newStartPoint = StartPoint; 
        List<SegmentViewModel> newSegments = new ();
        CanvasCoordinate segmentPoint0 = new CanvasCoordinate(0, 0);
        bool isFirst = true;
        foreach (var segment in Segments)
        {
            if (isFirst)
            {
                newSegments.Add(segment.GetTopAlignmentWithRespectTo(thickness, segmentPoint0, out newStartPoint));
                isFirst = false;
            }
            else
            {
                newSegments.Add(segment.GetTopAlignmentWithRespectTo(thickness, segmentPoint0));
            }
            segmentPoint0 = segment.LastPoint;
        }
        return new PathPolygonViewModel(Position, newStartPoint, newSegments);
    }
    private PathPolygonViewModel(CanvasCoordinate position, CanvasCoordinate startPoint, IEnumerable<SegmentViewModel> segments)
    {
        Position = position;
        StartPoint = startPoint;
        Segments = segments;
    }
}
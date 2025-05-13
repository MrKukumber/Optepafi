using System;
using System.Globalization;
using Avalonia;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.Models.Utils.Shapes.Segments;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class LineSegmentViewModel : SegmentViewModel
{
    public LineSegmentViewModel(LineSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsTopleftVertex)
    {
        Point1 = segment.Point1.ToCanvasCoordinate(mapsTopleftVertex) - relativePosition;
    }
    public CanvasCoordinate Point1 { get; } 
    public override string GetStringRep()
    {
        Point relativePoint1 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point1);
        return "L " + relativePoint1.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint1.Y.ToString(CultureInfo.InvariantCulture) + " ";    
    }
    public override CanvasCoordinate LastPoint => Point1;

    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0)
    {
        var tan_1 = dL(1, point0);
        var norm_1 = tan_1.Rotate((float)Math.PI / 2);
        return new LineSegmentViewModel( Point1 + norm_1 * (thickness / norm_1.GetSize()) );
    }
    
    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0, out CanvasCoordinate alignedPoint0)
    {
        var tan_0 = dL(0, point0);
        var norm_0 = tan_0.Rotate((float)Math.PI / 2);
        alignedPoint0 = point0 * (thickness / norm_0.GetSize());
        return GetTopAlignmentWithRespectTo(thickness, point0);
    }
    
    private LineSegmentViewModel(CanvasCoordinate point1)
    {
        Point1 = point1;
    }
    private CanvasCoordinate dL(double t, CanvasCoordinate point0) => Point1 - point0;
}
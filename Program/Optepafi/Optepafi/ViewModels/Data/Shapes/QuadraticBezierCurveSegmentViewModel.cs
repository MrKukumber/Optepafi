using System;
using System.Globalization;
using Avalonia;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes.Segments;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

public class QuadraticBezierCurveSegmentViewModel : SegmentViewModel
{
    
    public QuadraticBezierCurveSegmentViewModel(QuadraticBezierCurveSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsTopLeftVertex)
    {
        Point1 =  segment.Point1.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
        Point2 = segment.Point2.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
    }
    public CanvasCoordinate Point1 { get; }
    public CanvasCoordinate Point2 { get; }
    public override string GetStringRep()
    {
        Point relativePoint1 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point1);
        Point relativePoint2 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point2);
        return "Q " + relativePoint1.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint1.Y.ToString(CultureInfo.InvariantCulture) + " "
               + relativePoint2.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint2.Y.ToString(CultureInfo.InvariantCulture) + " ";
    }

    public override CanvasCoordinate LastPoint => Point2;
    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0)
    {
        var tan_0_5 = dB(0.5, point0);
        var tan_1 = dB(1, point0);
        var norm_0_5 = tan_0_5.Rotate((float)Math.PI / 2);
        var norm_1 = tan_1.Rotate((float)Math.PI / 2);
        return new QuadraticBezierCurveSegmentViewModel(
            Point1 + norm_0_5 * (thickness / norm_0_5.Size()),
            Point2 + norm_1 * (thickness / norm_1.Size()));
    }

    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0, out CanvasCoordinate alignedPoint0)
    {
        var tan_0 = dB(0, point0);
        var tan_0_5 = dB(0.5, point0);
        var tan_1 = dB(1, point0);
        var norm_0 = tan_0.Rotate((float)Math.PI / 2);
        var norm_0_5 = tan_0_5.Rotate((float)Math.PI / 2);
        var norm_1 = tan_1.Rotate((float)Math.PI / 2);
        alignedPoint0 = point0 * (thickness / norm_0.Size());
        return new QuadraticBezierCurveSegmentViewModel(
            Point1 + norm_0_5 * (thickness / norm_0_5.Size()),
            Point2 + norm_1 * (thickness / norm_1.Size()));
    }
    
    private QuadraticBezierCurveSegmentViewModel(CanvasCoordinate point1, CanvasCoordinate point2)
    {
        Point1 = point1;
        Point2 = point2;
    }

    private CanvasCoordinate dB(double t, CanvasCoordinate point0) => 2 * (1 - t) * (Point1 - point0) 
                                                                      + 2 * t * (Point2 - Point1);
}
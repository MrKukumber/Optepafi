using System;
using System.Globalization;
using Avalonia;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.Models.Utils.Shapes.Segments;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class CubicBezierCurveSegmentViewModel : SegmentViewModel
{
    public CubicBezierCurveSegmentViewModel(CubicBezierCurveSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsTopLeftVertex)
    {
        Point1 =  segment.Point1.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
        Point2 = segment.Point2.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
        Point3 = segment.Point3.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
    }
    public CanvasCoordinate Point1 { get; }
    public CanvasCoordinate Point2 { get; }
    public CanvasCoordinate Point3 { get; }

    public override string GetStringRep()
    {
        Point relativePoint1 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point1);
        Point relativePoint2 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point2);
        Point relativePoint3 = CanvasCoordinateToAvaloniaPointConverter.Instance.Convert(Point3);
        return "C " + relativePoint1.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint1.Y.ToString(CultureInfo.InvariantCulture) + " "
               + relativePoint2.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint2.Y.ToString(CultureInfo.InvariantCulture) + " "
               + relativePoint3.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint3.Y.ToString(CultureInfo.InvariantCulture) + " ";
    }

    public override CanvasCoordinate LastPoint => Point3;


    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0)
    {
        var tan_0_5 = dB(0.5, point0);
        var tan_1 = dB(1, point0);
        var norm_0_5 = tan_0_5.Rotate((float)Math.PI / 2);
        var norm_1 = tan_1.Rotate((float)Math.PI / 2);
        return new CubicBezierCurveSegmentViewModel(
            Point1 + norm_0_5 * (thickness / norm_0_5.Size()), 
            Point2 + norm_0_5 * (thickness / norm_0_5.Size()),
            Point3 + norm_1 * (thickness / norm_1.Size()));
    }
    public override SegmentViewModel GetTopAlignmentWithRespectTo(int thickness, CanvasCoordinate point0, out CanvasCoordinate alignedPoint0)
    {
        var tan_0 = dB(0, point0);
        var norm_0 = tan_0.Rotate((float)Math.PI / 2);
        alignedPoint0 = point0 * (thickness / norm_0.Size()); 
        return GetTopAlignmentWithRespectTo(thickness, alignedPoint0);
    }
    private CubicBezierCurveSegmentViewModel(CanvasCoordinate point1, CanvasCoordinate point2, CanvasCoordinate point3)
    {
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;
    }
    private CanvasCoordinate dB(double t, CanvasCoordinate point0) => 3 * Math.Pow(1 - t, 2) * (Point1 - point0)
                                                                   + 6 * (1 - t) * t * (Point2 - Point1)
                                                                   + 3 * t * t * (Point3 - Point2);
}
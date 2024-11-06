using System.Globalization;
using Avalonia;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class CubicBezierCurveSegmentViewModel(CubicBezierCurveSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsTopLeftVertex) : SegmentViewModel
{
    static CanvasCoordinateToAvaloniaPointConverter _converter = new ();
    public CanvasCoordinate Point1 { get; }=  segment.Point1.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
    public CanvasCoordinate Point2 { get; }= segment.Point2.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;
    public CanvasCoordinate Point3 { get; }= segment.Point3.ToCanvasCoordinate(mapsTopLeftVertex) - relativePosition;

    public override string GetStringRep()
    {
        Point relativePoint1 = (Point)_converter.Convert(Point1);
        Point relativePoint2 = (Point)_converter.Convert(Point2);
        Point relativePoint3 = (Point)_converter.Convert(Point3);
        return "C " + relativePoint1.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint1.Y.ToString(CultureInfo.InvariantCulture) + " "
               + relativePoint2.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint2.Y.ToString(CultureInfo.InvariantCulture) + " "
               + relativePoint3.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint3.Y.ToString(CultureInfo.InvariantCulture) + " ";
    }

    public override CanvasCoordinate LastPoint => Point3;
}
using System.Globalization;
using Avalonia;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;
using Optepafi.Views.Utils;

namespace Optepafi.ViewModels.Data.Shapes;

    //TODO: comment
public class LineSegmentViewModel(LineSegment segment, CanvasCoordinate relativePosition, MapCoordinates mapsTopleftVertex) : SegmentViewModel
{
    static CanvasCoordinateToAvaloniaPointConverter _converter = new ();
    public CanvasCoordinate Point1 { get; } = segment.Point1.ToCanvasCoordinate(mapsTopleftVertex) - relativePosition;
    public override string GetStringRep()
    {
        
        Point relativePoint1 = (Point)_converter.Convert(Point1);
        return "L " + relativePoint1.X.ToString(CultureInfo.InvariantCulture) + "," + relativePoint1.Y.ToString(CultureInfo.InvariantCulture) + " ";    
    }
    
    public override CanvasCoordinate LastPoint => Point1;
}
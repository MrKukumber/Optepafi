using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;

namespace Optepafi.ModelViews.Utils;

public static class PointsToMapCoordinatesConverter
{
    public static IEnumerable<MapCoordinate> ConvertAccordingTo(IEnumerable<(int leftPos, int bottomPos)> points, GraphicsArea area)
    {
        return points
            .Select(point => new MapCoordinate(
                point.leftPos - area.LeftBottomVertex.XPos, 
                point.bottomPos - area.LeftBottomVertex.YPos));
    }
}
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.ModelViews.Utils;

public static class PointsToLegsConverter
{
    public static List<Leg> ConvertAccordingTo(IList<(int leftPos, int bottomPos)> points, GraphicsArea area)
    {
        int i = 0;
        List<Leg> result = new();
        while (i + 1 < points.Count)
        {
            result.Add(new Leg(
                new MapCoordinate(
                    points[i].leftPos - area.LeftBottomVertex.XPos, 
                    points[i].bottomPos - area.LeftBottomVertex.YPos), 
                new MapCoordinate(
                    points[i+1].leftPos - area.LeftBottomVertex.XPos,
                    points[i+1].bottomPos - area.LeftBottomVertex.YPos)));
            i++;
        }
        return result;
    }
}
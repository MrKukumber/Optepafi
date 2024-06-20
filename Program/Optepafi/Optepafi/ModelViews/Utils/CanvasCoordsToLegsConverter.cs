using System.Collections.Generic;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Utils;

public static class CanvasCoordsToLegsConverter
{
    public static List<Leg> ConvertAccordingTo(IList<CanvasCoordinate> points, GraphicsArea area)
    {
        int i = 0;
        List<Leg> result = new();
        while (i + 1 < points.Count)
        {
            result.Add(new Leg(points[i].ToMapCoordinate(area.LeftBottomVertex),
                points[i + 1].ToMapCoordinate(area.LeftBottomVertex)));
            i++;
        }
        return result;
    }
}
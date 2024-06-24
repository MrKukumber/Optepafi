using System.Collections.Generic;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Utils;

/// <summary>
/// Static class which provides static conversion method from list of tracks <c>CanvasCoordinate</c>s to list of corresponding <c>Leg</c>s.
/// </summary>
public static class CanvasCoordsToLegsConverter
{
    /// <summary>
    /// Method for conversion of list of tracks <c>CanvasCoordinate</c>s to list of corresponding <c>Leg</c>s.
    /// <c>CanvasCoordinate</c>s are converted to <c>MapCoordinate</c>s according to provided graphics areas left-bottom vertex.
    /// </summary>
    /// <param name="track">Coordinates of track to be converted to legs.</param>
    /// <param name="area">Area according to which are canvas coordinates converted to map coordinates</param>
    /// <returns>New list of created legs.</returns>
    public static List<Leg> ConvertAccordingTo(IList<CanvasCoordinate> track, GraphicsArea area)
    {
        int i = 0;
        List<Leg> result = new();
        while (i + 1 < track.Count)
        {
            result.Add(new Leg(track[i].ToMapCoordinate(area.LeftBottomVertex),
                track[i + 1].ToMapCoordinate(area.LeftBottomVertex)));
            i++;
        }
        return result;
    }
}
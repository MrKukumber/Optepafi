using System.Collections.Generic;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data;

namespace Optepafi.ModelViews.Utils;

/// <summary>
/// Static class which provides static conversion method from list of tracks <c>CanvasCoordinate</c>s to list of corresponding <c>Leg</c>s.  
/// </summary>
public static class CanvasCoordsLegsConversionExtension
{
    /// <summary>
    /// Method for conversion of list of tracks <c>CanvasCoordinate</c>s to list of corresponding <c>Leg</c>s.
    /// 
    /// <c>CanvasCoordinate</c>s are converted to <c>MapCoordinate</c>s according to provided graphics areas left-bottom vertex.  
    /// </summary>
    /// <param name="track">Coordinates of track to be converted to legs.</param>
    /// <param name="area">Area according to which are canvas coordinates converted to map coordinates</param>
    /// <returns>New list of created legs.</returns>
    public static List<Leg> ConvertToLegs(this IList<CanvasCoordinate> track, GraphicsArea area)
    {
        int i = 0;
        List<Leg> result = new();
        while (i + 1 < track.Count)
        {
            result.Add(new Leg(track[i].ToMapCoordinate(new MapCoordinates(area.BottomLeftVertex.XPos, area.TopRightVertex.YPos)),
                track[++i].ToMapCoordinate(new MapCoordinates(area.BottomLeftVertex.XPos, area.TopRightVertex.YPos))));
        }
        return result;
    }

    public static List<CanvasCoordinate> ConvertToCanvasCoords(this IList<Leg> legs, GraphicsArea area)
    {
        int i = 0;
        List<CanvasCoordinate> result = new();
        while (i < legs.Count)
        {
            result.Add(legs[i++].Start.ToCanvasCoordinate(new MapCoordinates(area.BottomLeftVertex.XPos, area.TopRightVertex.YPos)));
        }
        result.Add(legs[i - 1].Finish.ToCanvasCoordinate(new MapCoordinates(area.BottomLeftVertex.XPos, area.TopRightVertex.YPos)));
        return result;
    }
}
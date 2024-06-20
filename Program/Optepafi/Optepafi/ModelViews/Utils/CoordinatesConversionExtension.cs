using Optepafi.Models.MapMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Utils;

public static class CoordinatesConversionExtension
{
    public static MapCoordinate ToMapCoordinate(this CanvasCoordinate canvasCoordinate,
        MapCoordinate mapLeftBottomVertex)
    {
        return new MapCoordinate(canvasCoordinate.LeftPos + mapLeftBottomVertex.XPos,
            canvasCoordinate.BottomPos + mapLeftBottomVertex.YPos);
    }

    public static CanvasCoordinate ToCanvasCoordinate(this MapCoordinate mapCoordinate,
        MapCoordinate mapLeftBottomVertex)
    {
        return new CanvasCoordinate(mapCoordinate.XPos - mapLeftBottomVertex.XPos,
            mapCoordinate.YPos - mapLeftBottomVertex.YPos);
    }
}
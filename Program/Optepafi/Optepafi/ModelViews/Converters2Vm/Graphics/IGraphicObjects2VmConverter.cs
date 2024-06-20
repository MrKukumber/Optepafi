using Optepafi.Models.MapMan;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ModelViews.Converters.Graphics;

public interface IGraphicObjects2VmConverter<in TGraphicsObject> : IGraphicObjects2VmConverter
{
    public GraphicObjectViewModel ConvertToViewModel(TGraphicsObject graphicsObject, MapCoordinate mapsLeftBottomVertex);
}

public interface IGraphicObjects2VmConverter
{
    
}
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ModelViews.Converters.Graphics;

public interface IGraphicObjects2VmConverter<in TGraphicsObject> : IGraphicObjects2VmConverter
{
    public GraphicObjectViewModel ConvertToViewModel(TGraphicsObject graphicsObject, int minimalXPosition, int minimalYPosition);
}

public interface IGraphicObjects2VmConverter
{
    
}
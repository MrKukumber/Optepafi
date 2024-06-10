using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ModelViews.Graphics;

public interface IGraphicObjects2VmConverter<in TGraphicsObject> : IGraphicObjects2VMConverter
{
    public GraphicObjectViewModel ConvertToViewModel(TGraphicsObject graphicsObject, int minimalXPosition, int minimalYPosition);
}

public interface IGraphicObjects2VMConverter
{
    
}
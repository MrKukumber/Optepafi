using Optepafi.Models.Graphics.Objects;

namespace Optepafi.Models.Graphics;

public interface IGraphicsObjectCollector
{

    public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject;
}
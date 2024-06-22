using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

public interface IGraphicObjectEditor : IGraphicObjectCollector
{
    public void Remove(IGraphicObject graphicObject);
    
    public void Clear();

    public bool Contains(IGraphicObject graphicObject);
}
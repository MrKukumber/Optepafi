using DynamicData;
using Optepafi.Models.Graphics.Objects;

namespace Optepafi.ModelViews.Graphics.GraphicsSources;

public interface IGraphicsSource
{
    public SourceList<IGraphicObject> GraphicObjects { get; }
    public int MinimalXPos { get; }
    public int MinimalYPos { get; }
    public int MaximalXPos { get; }
    public int MaximalYPos { get; }
}
using DynamicData;
using Optepafi.Models.Graphics.Objects;

namespace Optepafi.Models.Graphics.Sources;

public interface IGraphicsSource
{
    public SourceList<IGraphicObject> GraphicObjects { get; }
}
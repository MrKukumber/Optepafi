using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.Models.Graphics.Sources;

public interface IGroundGraphicsSource : IGraphicsSource
{
    GraphicsArea GraphicsArea { get; }
}
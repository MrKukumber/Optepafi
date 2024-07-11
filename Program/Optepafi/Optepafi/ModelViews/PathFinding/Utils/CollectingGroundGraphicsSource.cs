using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Sources;

namespace Optepafi.ModelViews.PathFinding.Utils;

/// <summary>
/// Collecting graphics source that is also ground graphics source.
/// For more information on collecting graphic sources see <see cref="CollectingGraphicsSource"/>.
/// For more information on ground graphics sources see <see cref="IGroundGraphicsSource"/>.
/// </summary>
public class CollectingGroundGraphicsSource : CollectingGraphicsSource, IGroundGraphicsSource
{
    public CollectingGroundGraphicsSource(GraphicsArea graphicsArea)
    {
        GraphicsArea = graphicsArea;
    }
    
    /// <inheritdoc cref="IGroundGraphicsSource.GraphicsArea"/>
    public GraphicsArea GraphicsArea { get; }
}
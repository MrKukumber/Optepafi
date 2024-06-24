using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.CodeAnalysis.CSharp;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

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
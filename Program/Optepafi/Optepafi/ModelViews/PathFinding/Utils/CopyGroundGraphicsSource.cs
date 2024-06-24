using System.Dynamic;
using System.Threading.Tasks;
using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

/// <summary>
/// Represents ground graphics source that can be created by deep copy of other ground graphics source.
/// For more information on ground graphics sources see <see cref="IGroundGraphicsSource"/>.
/// </summary>
public class CopyGroundGraphicsSource : IGroundGraphicsSource
{
    
    private CopyGroundGraphicsSource(SourceList<IGraphicObject> graphicObjects, GraphicsArea graphicsArea)
    {
        GraphicObjects = graphicObjects;
        GraphicsArea = graphicsArea;
    }

    /// <summary>
    /// Method for creation of ground graphics source by copying the other one. Graphics objects are copied asynchronously one after another. 
    /// </summary>
    /// <param name="groundGraphicsSource">Ground graphics source to be copied.</param>
    /// <returns>New instance of copied graphic source.</returns>
    public static CopyGroundGraphicsSource ParallelCopy(IGroundGraphicsSource groundGraphicsSource)
    {
        var graphicsObjects = new SourceList<IGraphicObject>();
        CopyGroundGraphicsSource copyGroundGraphicsSource = new CopyGroundGraphicsSource(graphicsObjects, groundGraphicsSource.GraphicsArea);
        Task.Run(() =>
        {
            foreach (var graphicObject in groundGraphicsSource.GraphicObjects.Items)
            {
                graphicsObjects.Add(graphicObject);
            }
        });
        return copyGroundGraphicsSource;
    }

    /// <inheritdoc cref="IGraphicsSource.GraphicObjects"/>
    public SourceList<IGraphicObject> GraphicObjects { get; }
    /// <inheritdoc cref="IGroundGraphicsSource.GraphicsArea"/>
    public GraphicsArea GraphicsArea { get; }
}
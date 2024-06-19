using System.Dynamic;
using System.Threading.Tasks;
using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

public class CopyGroundGraphicsSource : IGroundGraphicsSource
{
    
    private CopyGroundGraphicsSource(SourceList<IGraphicObject> graphicObjects, GraphicsArea graphicsArea)
    {
        GraphicObjects = graphicObjects;
        GraphicsArea = graphicsArea;
    }

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

    public SourceList<IGraphicObject> GraphicObjects { get; }
    public GraphicsArea GraphicsArea { get; }
}
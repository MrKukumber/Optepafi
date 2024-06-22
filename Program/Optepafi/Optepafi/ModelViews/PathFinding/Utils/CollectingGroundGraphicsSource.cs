using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.CodeAnalysis.CSharp;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

public class CollectingGroundGraphicsSource : IGroundGraphicsSource
{
    public CollectingGroundGraphicsSource(GraphicsArea graphicsArea)
    {
        GraphicObjects = new SourceList<IGraphicObject>();
        GraphicsArea = graphicsArea;
    }

    public SourceList<IGraphicObject> GraphicObjects { get; }
    public GraphicsArea GraphicsArea { get; }
    public virtual IGraphicObjectCollector Collector => new GraphicObjectCollector(GraphicObjects);
    protected class GraphicObjectCollector : IGraphicObjectCollector
    {
        private SourceList<IGraphicObject> _graphicObjectSourceList;

        public GraphicObjectCollector(SourceList<IGraphicObject> graphicObjectSourceList)
        {
            _graphicObjectSourceList = graphicObjectSourceList;
        }
        public void Add(IGraphicObject graphicObject)
        {
            _graphicObjectSourceList.Add(graphicObject);
        }
        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicObjectSourceList.AddRange(graphicObjects);
        }
    }
        
}
using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.Models.Graphics.Sources;

public class CollectingGroundGraphicsSource : IGroundGraphicsSource
{
    public CollectingGroundGraphicsSource(GraphicsArea graphicsArea)
    {
        GraphicObjects = new SourceList<IGraphicObject>();
        GraphicsArea = graphicsArea;
    }
    public SourceList<IGraphicObject> GraphicObjects { get; }
    public GraphicsArea GraphicsArea { get; }
    public IGraphicsObjectCollector Collector => new GraphicsObjectCollector(GraphicObjects);
    private class GraphicsObjectCollector : IGraphicsObjectCollector
    {
        private SourceList<IGraphicObject> _graphicObjectSourceList;

        public GraphicsObjectCollector(SourceList<IGraphicObject> graphicObjectSourceList)
        {
            _graphicObjectSourceList = graphicObjectSourceList;
        }
        public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject
        {
            _graphicObjectSourceList.Add(graphicObject);
        }
    }
        
}
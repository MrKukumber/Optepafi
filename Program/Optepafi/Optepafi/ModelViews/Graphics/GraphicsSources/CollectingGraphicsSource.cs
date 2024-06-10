using DynamicData;
using Optepafi.Models.Graphics;
using Optepafi.Models.Graphics.Objects;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ModelViews.Graphics.GraphicsSources;

public class CollectingGraphicsSource : IGraphicsSource
{
    public CollectingGraphicsSource(int minXPos, int minYPos, int maxXPos, int maxYPos)
    {
        GraphicObjects = new SourceList<IGraphicObject>();
        MinimalXPos = minXPos;
        MinimalYPos = minYPos;
        MaximalXPos = maxXPos;
        MaximalYPos = maxYPos;
    }
    public SourceList<IGraphicObject> GraphicObjects { get; }
    public int MinimalXPos { get; }
    public int MinimalYPos { get; }
    public int MaximalXPos { get; }
    public int MaximalYPos { get; }

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
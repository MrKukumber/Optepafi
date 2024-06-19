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
    public virtual IGraphicsObjectCollector Collector => new GraphicsObjectCollector(GraphicObjects);
    protected class GraphicsObjectCollector : IGraphicsObjectCollector
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
        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicObjectSourceList.AddRange(graphicObjects);
        }
    }
        
}
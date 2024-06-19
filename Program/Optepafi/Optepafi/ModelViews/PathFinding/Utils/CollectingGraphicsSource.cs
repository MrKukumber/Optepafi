using System.Collections.Generic;
using System.Drawing;
using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

public class CollectingGraphicsSource : IGraphicsSource
{
    public CollectingGraphicsSource()
    {
        GraphicObjects = new SourceList<IGraphicObject>();
    }
    public SourceList<IGraphicObject> GraphicObjects { get; }
    public virtual IGraphicsObjectCollector Collector => new GraphicsObjectCollector(GraphicObjects);

    protected class GraphicsObjectCollector : IGraphicsObjectCollector
    {
        private SourceList<IGraphicObject> _graphicObjectSource;

        public GraphicsObjectCollector(SourceList<IGraphicObject> graphicObjectSource)
        {
            _graphicObjectSource = graphicObjectSource;
        }

        public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject
        {
            _graphicObjectSource.Add(graphicObject);
        }
        
        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicObjectSource.AddRange(graphicObjects);
        }
    }
}
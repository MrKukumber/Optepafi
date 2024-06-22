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
    public virtual IGraphicObjectCollector Collector => new GraphicObjectCollector(GraphicObjects);

    protected class GraphicObjectCollector : IGraphicObjectCollector
    {
        private SourceList<IGraphicObject> _graphicObjectSource;

        public GraphicObjectCollector(SourceList<IGraphicObject> graphicObjectSource)
        {
            _graphicObjectSource = graphicObjectSource;
        }

        public void Add(IGraphicObject graphicObject)
        {
            _graphicObjectSource.Add(graphicObject);
        }
        
        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicObjectSource.AddRange(graphicObjects);
        }
    }
}
using System.Collections.Generic;
using DynamicData;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.GraphicsMan.Sources;

namespace Optepafi.ModelViews.PathFinding.Utils;

/// <summary>
/// Graphics source that is able to provide collector by which its graphic objects are collected.
/// Collected objects are directly appended into <c>GraphicObject</c> source list, so they can be immediately displayed to user.
/// For more information on graphics sources see <see cref="IGraphicsSource"/>.
/// </summary>
public class CollectingGraphicsSource : IGraphicsSource
{
    /// <inheritdoc cref="IGraphicsSource.GraphicObjects"/>
    public SourceList<IGraphicObject> GraphicObjects { get; } = new SourceList<IGraphicObject>();
    
    /// <summary>
    /// Collector by which graphic objects of this class can be collected.
    /// </summary>
    public virtual IGraphicObjectCollector Collector => new GraphicObjectCollector(GraphicObjects);

    /// <summary>
    /// Collector implementation which append added graphic objects directly to the <c>GraphicObjects</c> source list.
    /// </summary>
    protected class GraphicObjectCollector : IGraphicObjectCollector
    {
        
        /// <summary>
        /// Graphics sources source list to which added objects shall be appended.
        /// </summary>
        private SourceList<IGraphicObject> _graphicObjectSource;

        public GraphicObjectCollector(SourceList<IGraphicObject> graphicObjectSource)
        {
            _graphicObjectSource = graphicObjectSource;
        }
        
        /// <inheritdoc cref="IGraphicObjectCollector.Add"/>
        /// <remarks>
        /// Appends added objects directly to provided source list.
        /// </remarks>
        public void Add(IGraphicObject graphicObject)
        {
            _graphicObjectSource.Add(graphicObject);
        }
        /// <inheritdoc cref="IGraphicObjectCollector.AddRange"/>
        /// <remarks>
        /// Appends added object ranges directly to provided source list.
        /// </remarks>
        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicObjectSource.AddRange(graphicObjects);
        }
    }
}
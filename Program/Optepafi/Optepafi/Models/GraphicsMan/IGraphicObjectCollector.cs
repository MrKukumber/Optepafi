using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects;

namespace Optepafi.Models.GraphicsMan;

/// <summary>
/// Represents collector for graphic objects. It is used in graphics aggregators for collecting of aggregated graphic objects.
/// Its implementations can contain inner logic that enables processing of these objects right after their addition to collector.
/// </summary>
public interface IGraphicObjectCollector
{
    /// <summary>
    /// Method for adding of graphic object to collector.
    /// </summary>
    /// <param name="graphicObject">Graphic object to be added.</param>
    public void Add(IGraphicObject graphicObject); 
    /// <summary>
    /// Method for adding range of graphic objects to collector.
    /// </summary>
    /// <param name="graphicObjects">Graphic object range to be added.</param>
    public void AddRange(IEnumerable<IGraphicObject> graphicObjects);
}
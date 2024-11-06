using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects;

namespace OptepafiTests.Models.Graphics;

public class SimpleCollector : IGraphicObjectCollector
{
    List<IGraphicObject> _graphicObjects = new();
    public void Add(IGraphicObject graphicObject)
    {
        _graphicObjects.Add(graphicObject);
    }
    public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
    {
        _graphicObjects.AddRange(graphicObjects);
    }
}
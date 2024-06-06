using System.Collections.Generic;
using Optepafi.Models.Graphics;

namespace Optepafi.ModelViews.Graphics.Collectors;

public interface IGraphicsObjectCollector
{

    public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject;
    public void AddRange<TGraphicObject>(IEnumerable<TGraphicObject> graphicObjects) where TGraphicObject : IGraphicObject;
}
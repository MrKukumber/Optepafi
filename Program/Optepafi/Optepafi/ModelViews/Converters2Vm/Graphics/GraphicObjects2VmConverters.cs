using System;
using System.Collections.Generic;
using System.Linq;
using Optepafi.ModelViews.Converters.Graphics.MapObjects;
using Optepafi.ModelViews.Converters.Graphics.PathObjects;
using Optepafi.ModelViews.Converters.Graphics.SearchingReportObjects;

namespace Optepafi.ModelViews.Converters.Graphics;

public static class GraphicObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(MapObjects2VmConverters.Converters)
            .Concat(PathObjects2VmConverters.Converters)
            .Concat(SearchingStateObjects2VmConverters.Converters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}
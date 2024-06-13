using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters.Graphics.MapObjects;

public static class MapObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(TextMapObjects2VmConverters.Converters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}
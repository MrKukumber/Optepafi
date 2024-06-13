using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters.Graphics.PathObjects;

public static class PathObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(SmileyFacePathObjects2VmConverters.Converters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}


using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters.Graphics.SearchingReportObjects;

public static class SearchingStateObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.SearchingStateObjects;

/// <summary>
/// Static class which contains dictionary of searching state graphic object to ViewModel converters.
/// It should contain all such converters. It is directly concatenated to root dictionary in <see cref="GraphicObjects2VmConverters"/> which is used by application logic for appropriate converter search.
/// </summary>
public static class SearchingStateObjects2VmConverters
{
    /// <summary>
    /// Dictionary of searching state graphic objects to ViewModel converters.
    /// </summary>
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}
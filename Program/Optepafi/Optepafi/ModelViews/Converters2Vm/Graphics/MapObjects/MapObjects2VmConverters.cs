using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapObjects;


/// <summary>
/// Static class which contains dictionary of map graphic object to ViewModel converters.
/// 
/// It should contain all such converters. It is directly concatenated to root dictionary in <see cref="GraphicObjects2VmConverters"/> which is used by application logic for appropriate converter search.  
/// </summary>
public static class MapObjects2VmConverters
{
    
    /// <summary>
    /// Dictionary of map graphic objects to ViewModel converters.
    /// </summary>
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(TextMapObjects2VmConverters.Converters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}
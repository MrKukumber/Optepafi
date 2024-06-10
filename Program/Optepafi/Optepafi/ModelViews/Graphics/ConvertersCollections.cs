using System;
using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.ModelViews.Graphics.MapObjectConverters;
using Optepafi.ModelViews.Graphics.PathObjectConverters;

namespace Optepafi.ModelViews.Graphics;

public static class ConvertersCollections
{
    public static Dictionary<Type, IGraphicObjects2VMConverter> PathObjects2VmConverters =
        new Dictionary<Type, IGraphicObjects2VMConverter>()
            .Concat(SmileyFacePathObject2VmConverters.AllConverters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    
    public static Dictionary<Type, IGraphicObjects2VMConverter> MapObjects2VmConverters =
        new Dictionary<Type, IGraphicObjects2VMConverter>()
            .Concat(TextMapObject2VmConverters.AllConverters) //Add here any new MapObjectConverter
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    
    public static Dictionary<Type, IGraphicObjects2VMConverter> PathFindingObjects2VmConverters =
        MapObjects2VmConverters
            .Concat(PathObjects2VmConverters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    public static List<Dictionary<Type, IGraphicObjects2VMConverter>> AllCollections = [PathFindingObjects2VmConverters];
}
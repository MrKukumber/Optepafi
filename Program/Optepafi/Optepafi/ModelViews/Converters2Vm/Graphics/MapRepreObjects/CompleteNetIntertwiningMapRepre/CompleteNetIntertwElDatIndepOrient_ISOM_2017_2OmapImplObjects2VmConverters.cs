using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects.MapRepre.CompleteNetIntertwiningMapRepre;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.MapRepreObjects.CompleteNetIntertwiningMapRepre;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapRepreObjects.CompleteNetIntertwiningMapRepre;

public static class CompleteNetIntertwElDatIndepOrient_ISOM_2017_2OmapImplObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
        {typeof(VertexObject), VertexObject2VmConstructor.Instance},
        {typeof(EdgeObject), EdgeObject2VmConstructor.Instance},
    };
}

public class VertexObject2VmConstructor : IGraphicObjects2VmConverter<VertexObject>
{
    public static VertexObject2VmConstructor Instance = new();
    private VertexObject2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(VertexObject graphicsObject, MapCoordinates mapsTopLeftVertex) => new VertexObjectViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EdgeObject2VmConstructor : IGraphicObjects2VmConverter<EdgeObject>
{
    
    public static EdgeObject2VmConstructor Instance = new();
    private EdgeObject2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(EdgeObject graphicsObject, MapCoordinates mapsTopLeftVertex) => new EdgeObjectViewModel(graphicsObject, mapsTopLeftVertex);
}

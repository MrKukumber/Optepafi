using System;
using System.Collections.Generic;
using DynamicData;
using DynamicData.Binding;
using Optepafi.Models.Graphics;
using Optepafi.ViewModels.Graphics;

namespace Optepafi.ModelViews.Graphics.Collectors;

public class GraphicsObjects2VmConvertingCollector : IGraphicsObjectCollector
{
    private ICollection<GraphicObjectViewModel> _graphicObjectViewModels;
    private Dictionary<Type, IGraphicObjects2VMConverter> _viewModelConverters;

    private int _minimalXPosition;
    private int _minimalYPosition;
    public HashSet<Type> NoConverterGraphicObjectTypes { get; } = new();
    
    public GraphicsObjects2VmConvertingCollector(ICollection<GraphicObjectViewModel> graphicObjectViewModels,
        Dictionary<Type, IGraphicObjects2VMConverter> viewModelConverters, int minimalXPosition = 0, int minimalYPosition = 0)
    {
        _graphicObjectViewModels = graphicObjectViewModels;
        _viewModelConverters = viewModelConverters;
        _minimalXPosition = minimalXPosition;
        _minimalYPosition = minimalYPosition;
    }
    

    public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject
    {
        if(_viewModelConverters[typeof(TGraphicObject)] is IGraphicObjects2VmConverter<TGraphicObject> converter)
            _graphicObjectViewModels.Add(converter.ConvertToViewModel(graphicObject, _minimalXPosition, _minimalYPosition));
        else
        {
            NoConverterGraphicObjectTypes.Add(typeof(TGraphicObject));
            //TODO: mozno lognut absenciu converteru nejak
        } 
    }

    public void AddRange<TGraphicObject>(IEnumerable<TGraphicObject> graphicObjects) where TGraphicObject : IGraphicObject
    {
        ObservableCollectionExtended<TGraphicObject> o = new();
        SourceList<TGraphicObject> l = new();
        l.AddRange(graphicObjects);

    }
}
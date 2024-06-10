using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Optepafi.Models.Graphics;
using Optepafi.ModelViews.Graphics;
using Optepafi.ModelViews.Graphics.GraphicsSources;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Graphics;

public class GraphicsSourceViewModel : DataViewModel<IGraphicsSource>, IGraphicObjectGenericVisitor<GraphicObjectViewModel?>
{
    protected override IGraphicsSource Data => GraphicsSource;
    
    private readonly ReadOnlyObservableCollection<GraphicObjectViewModel?> _graphicObjectCollection;
    public ReadOnlyObservableCollection<GraphicObjectViewModel?> GraphicObjectsCollection => _graphicObjectCollection;
    public IGraphicsSource GraphicsSource { get; }
    public GraphicsSourceViewModel(IGraphicsSource graphicsSource) 
    {
        GraphicsSource = graphicsSource;
        graphicsSource.GraphicObjects
            .Connect()
            .Transform(graphicObject => graphicObject.AcceptGeneric(this))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _graphicObjectCollection)
            .Subscribe();
    }

    GraphicObjectViewModel? IGraphicObjectGenericVisitor<GraphicObjectViewModel?>.GenericVisit<TGraphicObject>(TGraphicObject graphicObject)
    {
        foreach(var convertersCollection in ConvertersCollections.AllCollections)
        {
            if (convertersCollection[typeof(TGraphicObject)] is IGraphicObjects2VmConverter<TGraphicObject> converter)
                return converter.ConvertToViewModel(graphicObject, GraphicsSource.MinimalXPos, GraphicsSource.MinimalYPos);
        }
        //TODO: lognut ked neni pritomny konverter
        return null;
    }
    public int GraphicsWidth => GraphicsSource.MaximalXPos - GraphicsSource.MinimalXPos;
    public int GraphicsHeight => GraphicsSource.MaximalYPos - GraphicsSource.MinimalYPos;
}
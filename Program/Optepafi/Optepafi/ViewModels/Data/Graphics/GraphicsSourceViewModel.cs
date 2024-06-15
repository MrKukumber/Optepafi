using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Optepafi.Models.Graphics;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.ModelViews.Converters;
using Optepafi.ModelViews.Converters.Graphics;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Graphics;

public sealed class GraphicsSourceViewModel : DataViewModel<IGraphicsSource>, IGraphicObjectGenericVisitor<GraphicObjectViewModel?, MapCoordinate>
{
    protected override IGraphicsSource Data => GraphicsSource;
    
    private readonly ReadOnlyObservableCollection<GraphicObjectViewModel?> _graphicObjectCollection;
    public ReadOnlyObservableCollection<GraphicObjectViewModel?> GraphicObjectsCollection => _graphicObjectCollection;
    public IGraphicsSource GraphicsSource { get; }
    
    public GraphicsSourceViewModel(IGroundGraphicsSource groundGraphicsSource) : this(groundGraphicsSource, groundGraphicsSource) { }
    public GraphicsSourceViewModel(IGraphicsSource graphicsSource, IGroundGraphicsSource relatedGroundGraphicsSource)
    {
        GraphicsSource = graphicsSource;
        GraphicsWidth = relatedGroundGraphicsSource.GraphicsArea.TopRightVertex.XPos - relatedGroundGraphicsSource.GraphicsArea.LeftBottomVertex.XPos;
        GraphicsHeight = relatedGroundGraphicsSource.GraphicsArea.TopRightVertex.YPos - relatedGroundGraphicsSource.GraphicsArea.LeftBottomVertex.YPos;
        graphicsSource.GraphicObjects
            .Connect()
            .Transform(graphicObject => graphicObject.AcceptGeneric(this, relatedGroundGraphicsSource.GraphicsArea.LeftBottomVertex))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _graphicObjectCollection)
            .Subscribe();
    }

    GraphicObjectViewModel? IGraphicObjectGenericVisitor<GraphicObjectViewModel?, MapCoordinate>.GenericVisit<TGraphicObject>(TGraphicObject graphicObject, MapCoordinate leftBottomVertex)
    {
        if (GraphicObjects2VmConverters.Converters[typeof(TGraphicObject)] is IGraphicObjects2VmConverter<TGraphicObject> converter)
            return converter.ConvertToViewModel(graphicObject, leftBottomVertex.XPos, leftBottomVertex.YPos);
        //TODO: lognut ked neni pritomny konverter
        return null;
    }
    public int GraphicsWidth { get; }
    public int GraphicsHeight { get; }
}
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Converters2Vm.Graphics;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Graphics;

/// <summary>
/// ViewModel for any <c>IGraphicSource</c> type.
/// 
/// Ground graphics sources can be assigned to ViewModel on their own.  
/// Classic graphic source must be assigned together with responding ground graphic which is able to define the area of graphics.  
/// Graphic objects from source list of graphics source are bound to observable collection of appropriate ViewModels to which are graphic objects converted by suitable converters.  
/// For more information on data ViewModels see <see cref="DataViewModel"/>.  
/// </summary>
public sealed class GraphicsSourceViewModel : 
    DataViewModel, 
    IGraphicObjectGenericVisitor<GraphicObjectViewModel?, MapCoordinates>
{
    /// <summary>
    /// Creates ViewModel for ground graphic source.
    ///
    /// That is the same think as it would create ViewModel for graphics source which responds to itself.  
    /// </summary>
    /// <param name="groundGraphicsSource">Ground graphics source for which ViewModel is created.</param>
    public GraphicsSourceViewModel(IGroundGraphicsSource groundGraphicsSource) : this(groundGraphicsSource, groundGraphicsSource) { }
    /// <summary>
    /// Creates ViewModel for provided graphics source which responds to ground graphic source provided in second argument.
    /// 
    /// Ground graphic source holds and graphics area definition to which is graphic source tied.  
    /// Graphic objects from source list of graphics source are bound to observable collection of appropriate ViewModels to which are graphic objects converted by suitable converters.  
    /// Suitable converters are identified thanks to use of "generic visitor pattern" on graphic objects.  
    /// Generic visitor pattern reveals real type of graphic object to which is then in dictionary of all graphic object to ViewModel converters found the appropriate one.  
    /// For more information on generic visitor pattern see <see cref="IGraphicObjectGenericVisitor{TOut,TOtherParams}"/>.  
    /// </summary>
    /// <param name="graphicsSource">Graphics source for which ViewModel is created.</param>
    /// <param name="respondingGroundGraphicsSource">Ground graphics source to which converted graphics source respond.</param>
    public GraphicsSourceViewModel(IGraphicsSource graphicsSource, IGroundGraphicsSource respondingGroundGraphicsSource)
    {
        GraphicsWidth = respondingGroundGraphicsSource.GraphicsArea.TopRightVertex.XPos - respondingGroundGraphicsSource.GraphicsArea.BottomLeftVertex.XPos;
        GraphicsHeight = respondingGroundGraphicsSource.GraphicsArea.TopRightVertex.YPos - respondingGroundGraphicsSource.GraphicsArea.BottomLeftVertex.YPos;
        graphicsSource.GraphicObjects
            .Connect()
            .Transform(graphicObject => graphicObject.AcceptGeneric(this, 
                new MapCoordinates(respondingGroundGraphicsSource.GraphicsArea.BottomLeftVertex.XPos, respondingGroundGraphicsSource.GraphicsArea.TopRightVertex.YPos)))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _graphicObjectCollection)
            .Subscribe();
    }
    GraphicObjectViewModel? IGraphicObjectGenericVisitor<GraphicObjectViewModel?, MapCoordinates>.GenericVisit<TGraphicObject>(TGraphicObject graphicObject, MapCoordinates topLeftVertex)
    {
        if (GraphicObjects2VmConverters.Converters[typeof(TGraphicObject)] is IGraphicObjects2VmConverter<TGraphicObject> converter)
            return converter.ConvertToViewModel(graphicObject, topLeftVertex);
        //TODO: lognut ked neni pritomny konverter
        return null;
    }
    
    /// <summary>
    /// Backing field for GraphicObjectsCollection property.
    ///
    /// It is used for binding of graphic object source list of provided graphics source.
    /// </summary>
    private readonly ReadOnlyObservableCollection<GraphicObjectViewModel?> _graphicObjectCollection;
    /// <summary>
    /// Read only observable collection filled with ViewModels of graphic objects from graphics source for which was this ViewModel created..
    /// </summary>
    public ReadOnlyObservableCollection<GraphicObjectViewModel?> GraphicObjectsCollection => _graphicObjectCollection;
    
    /// <summary>
    /// Width of whole graphics.
    ///
    /// Aggregated from area of bounded ground graphics source or ground graphics source which responds to bounded graphics source.
    /// </summary>
    public int GraphicsWidth { get; }
    /// <summary>
    /// Height of whole graphics.
    ///
    /// Aggregated from area of bounded ground graphics source or ground graphics source which responds to bounded graphics source.
    /// </summary>
    public int GraphicsHeight { get; }
}
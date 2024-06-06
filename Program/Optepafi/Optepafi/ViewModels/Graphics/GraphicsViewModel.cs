using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Utils;
using ReactiveUI;

namespace Optepafi.ViewModels.Graphics;

public class GraphicsViewModel : ReactiveObject
{

    public ObservableCollection<GraphicObjectViewModel> GraphicObjectsCollection { get; }
    
    public GraphicsViewModel(int graphicsWidth, int graphicsHeight, ObservableCollection<GraphicObjectViewModel> graphicObjectsCollection)
    {
        GraphicsWidth = graphicsWidth;
        GraphicsHeight = graphicsHeight;
        GraphicObjectsCollection = graphicObjectsCollection;
    }

    public GraphicsViewModel(int graphicsWidth, int graphicsHeight) : this(graphicsWidth, graphicsHeight, new ObservableCollection<GraphicObjectViewModel>()){}
    
    private int _graphicsWidth;
    public int GraphicsWidth 
    { 
        get => _graphicsWidth; 
        set => this.RaiseAndSetIfChanged(ref _graphicsWidth, value); 
    }

    private int _graphicsHeight;
    public int GraphicsHeight
    {
        get => _graphicsHeight; 
        set => this.RaiseAndSetIfChanged(ref _graphicsHeight, value);
    }
}
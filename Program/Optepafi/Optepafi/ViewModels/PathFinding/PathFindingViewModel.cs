using System.Reactive;
using Optepafi.Models.MapMan;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data.Graphics;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingViewModel : ViewModelBase
{
    public PFPathFindingModelView PathFindingMv { get; }
    public PathFindingViewModel(PFPathFindingModelView pathFindingMv)
    {
        PathFindingMv = pathFindingMv;
    }

    private GraphicsSourceViewModel _mapGraphicsSource;
    public GraphicsSourceViewModel MapGraphicsSource
    {
        get => _mapGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _mapGraphicsSource, value);
    }

    private GraphicsSourceViewModel _reportsGraphicsSource;
    public GraphicsSourceViewModel ReportsGraphicsSource
    {
        get => _reportsGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _reportsGraphicsSource, value);
    }

    private string _graphicsProblemText;
    public string GraphicsProblemText
    {
        get => _graphicsProblemText;
        set => this.RaiseAndSetIfChanged(ref _graphicsProblemText, value);
    }
    
    

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Unit, Unit> FindPathCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> CleanUpCommand { get; }
    public ReactiveCommand<float, Unit> ZoomCommand { get; }
    public ReactiveCommand<float, Unit> UnZoomCommand { get; }
    public ReactiveCommand<int?, Unit> AddTrackPointCommand { get; }
    public ReactiveCommand<int?, Unit> RemoveTrackPointCommand { get; }
    
    
}
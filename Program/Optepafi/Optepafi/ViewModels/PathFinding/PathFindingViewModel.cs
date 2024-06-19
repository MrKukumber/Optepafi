using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls.Converters;
using DynamicData;
using Optepafi.Models.MapMan;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingViewModel : PathFindingViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }
    private PFPathFindingModelView _pathFindingMv; 

    private List<(int leftPos, int bottomPos)> _acceptedTrackPointList = new();
    private int _acceptedTrackPointsCount = 0;
    private int AcceptedTrackPointsCount
    {
        get => _acceptedTrackPointsCount;
        set => this.RaiseAndSetIfChanged(ref _acceptedTrackPointsCount, value);
    } 
    public PathFindingViewModel(PFPathFindingModelView pathFindingMv)
    {
        _pathFindingMv = pathFindingMv;
        Activator = new ViewModelActivator();

        IObservable<bool> isAcceptingTrack = this.WhenAnyValue(x => x.IsAcceptingTrack);
        IObservable<bool> isShowingPathReport = this.WhenAnyValue(x => x.IsShowingPathReport);
        // IObservable<bool> isAnyPointAccepted = this.WhenAnyObservable(
            // x => x.AddTrackPointCommand,
            // x => x.RemoveTrackPointCommand,
            // (_,_) => _acceptedTrackPointList.Count > 0);
        // IObservable<bool> isMoreThanOnePointAccepted = this.WhenAnyObservable(
            // x => x.AddTrackPointCommand,
            // x => x.RemoveTrackPointCommand,
            // (_,_) => _acceptedTrackPointList.Count > 1);
        IObservable<bool> isAnyPointAccepted = this.WhenAnyValue(
            x => x.AcceptedTrackPointsCount,
            count => count > 0);
        IObservable<bool> isMoreThanOnePointAccepted = this.WhenAnyValue(
            x => x.AcceptedTrackPointsCount,
            count => count > 1);
        
        GetMapGraphicsCommand = ReactiveCommand.Create(() =>
        {
            return pathFindingMv.GetGroundMapGraphics();
        });
        
        FindPathCommand = ReactiveCommand.CreateFromObservable(
            () => Observable
                .StartAsync(async cancellationToken =>
                {
                    IsAcceptingTrack = false;
                    IProgress<SearchingReportViewModel> searchingProgress = new Progress<SearchingReportViewModel>(
                        report => LastReport = report);
                    return await _pathFindingMv.FindPathAsync(_acceptedTrackPointList, searchingProgress, cancellationToken);
                })
                .TakeUntil(CancelSearchCommand)
                .TakeUntil(OnClosedCommand)
            , isAcceptingTrack.CombineLatest(isMoreThanOnePointAccepted, (x,y) => x && y)); 
        
        CancelSearchCommand = ReactiveCommand.Create(() => { }, FindPathCommand.IsExecuting);

        CleanUpPathReportCommand = ReactiveCommand.Create(() =>
        {
            IsShowingPathReport = false;
            LastReport = null;
        }, isShowingPathReport);
        
        AddTrackPointCommand = ReactiveCommand.CreateFromTask(async ((int leftPos, int bottomPos, int? index) parameters) =>
        {
            if(parameters.index is null) _acceptedTrackPointList.Add((parameters.leftPos, parameters.bottomPos));
            else _acceptedTrackPointList.Insert(parameters.index.Value, (parameters.leftPos, parameters.bottomPos));
            return await pathFindingMv.GetTrackGraphicsAsync(_acceptedTrackPointList);
        }, isAcceptingTrack);

        RemoveTrackPointCommand = ReactiveCommand.CreateFromTask(async (int? index) =>
        {
            if(index is null) _acceptedTrackPointList.RemoveAt(_acceptedTrackPointList.Count-1);
            else _acceptedTrackPointList.RemoveAt(index.Value);
             return await pathFindingMv.GetTrackGraphicsAsync(_acceptedTrackPointList);
        }, isAcceptingTrack.CombineLatest(isAnyPointAccepted, (x, y) => x && y ));

        ZoomCommand = ReactiveCommand.Create((float howMuch) =>
        {
            GraphicsScale += howMuch;
        });

        UnZoomCommand = ReactiveCommand.Create((float howMuch) =>
        {
            GraphicsScale -= howMuch;
        });
        
        ExitCommand = ReactiveCommand.Create(() => { }); 
        
        OnClosedCommand = ReactiveCommand.Create(() =>
        {
            pathFindingMv.OnClosed();
        });
        
        FindPathCommand.Subscribe(pathReport =>
        {
            IsShowingPathReport = true;
            LastReport = pathReport;
            if (pathReport is null) CleanUpPathReportCommand.Execute().Subscribe();
        });

        CleanUpPathReportCommand.Subscribe(_ => { IsAcceptingTrack = true; });

        AddTrackPointCommand.Subscribe(trackGraphicsSource =>
        {
            AcceptedTrackPointsCount = _acceptedTrackPointList.Count;
            TrackGraphicsSource = trackGraphicsSource;
        });
        RemoveTrackPointCommand.Subscribe(trackGraphicsSource =>
        {
            AcceptedTrackPointsCount = _acceptedTrackPointList.Count;
            TrackGraphicsSource = trackGraphicsSource;
        });

        _reportsGraphicsSource = this.WhenAnyValue(x => x.LastReport,
                lastReport => lastReport?.GraphicsSource)
            .ToProperty(this, nameof(ReportGraphicsSource));
        _scaledGraphicsWidth = this.WhenAnyValue(x => x.GraphicsWidth,
                x => x.GraphicsScale,
                (width, scale) => (int)(width * scale))
            .ToProperty(this, nameof(ScaledGraphicsWidth));
        _scaledGraphicsHeight = this.WhenAnyValue(x => x.GraphicsHeight,
                x => x.GraphicsScale,
                (height, scale) => (int)(height * scale))
            .ToProperty(this, nameof(ScaledGraphicsHeight));
        
        this.WhenActivated( (CompositeDisposable disposables) =>
        {
            GetMapGraphicsCommand.Execute().Subscribe(mapGraphicsSource =>
            {
                MapGraphicsSource = mapGraphicsSource;
                GraphicsWidth = MapGraphicsSource.GraphicsWidth;
                GraphicsHeight = MapGraphicsSource.GraphicsHeight;
            }).DisposeWith(disposables);
        });

    }

    private bool _isShowingPathReport = false;
    public bool IsShowingPathReport
    {
        get => _isShowingPathReport;
        set => this.RaiseAndSetIfChanged(ref _isShowingPathReport, value);
    }

    private bool _isAcceptingTrack = true;
    public bool IsAcceptingTrack
    {
        get => _isAcceptingTrack;
        set => this.RaiseAndSetIfChanged(ref _isAcceptingTrack, value);
    }


    private GraphicsSourceViewModel _mapGraphicsSource;
    public GraphicsSourceViewModel MapGraphicsSource
    {
        get => _mapGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _mapGraphicsSource, value);
    }

    private GraphicsSourceViewModel _trackGraphicsSource;

    public GraphicsSourceViewModel TrackGraphicsSource
    {
        get => _trackGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _trackGraphicsSource, value);
    }

    private IReportWithGraphicsViewModel? _lastReport;
    public IReportWithGraphicsViewModel? LastReport
    {
        get => _lastReport;
        set => this.RaiseAndSetIfChanged(ref _lastReport, value);
    }

    private ObservableAsPropertyHelper<GraphicsSourceViewModel?> _reportsGraphicsSource;
    public GraphicsSourceViewModel? ReportGraphicsSource => _reportsGraphicsSource.Value;

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

    private float _graphicsScale = 1;
    public float GraphicsScale
    {
        get => _graphicsScale;
        set => this.RaiseAndSetIfChanged(ref _graphicsScale, value);
    }

    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsWidth;
    public int ScaledGraphicsWidth => _scaledGraphicsWidth.Value;

    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsHeight;
    public int ScaledGraphicsHeight => _scaledGraphicsHeight.Value;

    private string? _graphicsProblemText;
    public string? GraphicsProblemText
    {
        get => _graphicsProblemText;
        set => this.RaiseAndSetIfChanged(ref _graphicsProblemText, value);
    }
    public ReactiveCommand<Unit, GraphicsSourceViewModel> GetMapGraphicsCommand { get; }
    public ReactiveCommand<Unit, PathReportViewModel?> FindPathCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> CleanUpPathReportCommand { get; }
    public ReactiveCommand<(int, int, int?), GraphicsSourceViewModel> AddTrackPointCommand { get; }
    public ReactiveCommand<int?, GraphicsSourceViewModel> RemoveTrackPointCommand { get; }
    public ReactiveCommand<float, Unit> ZoomCommand { get; }
    public ReactiveCommand<float, Unit> UnZoomCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public sealed override ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
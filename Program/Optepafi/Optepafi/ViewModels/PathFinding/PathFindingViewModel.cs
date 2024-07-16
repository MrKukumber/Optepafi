using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

/// <summary>
/// ViewModel which is responsible for control over sessions path finding itself.
/// 
/// Its tasks include:
/// 
/// - overseeing of correctness of user inputs. It secures consistency of path finding process by restricting availability of users actions.
/// - use of corresponding ModelViews methods for retrieving reports about results of path findings and graphic representations which can be shown to user
/// - rendering of resulting graphics for user. Includes mechanisms for scaling of graphics rendering.
/// - proper handling of sessions main windows closing. Informing ModelView about its occurence.
///
/// This ViewModel is activatable. That means when its corresponding View is attached to visual tree of application, function inserted into <c>WhenActivated</c>  method is called and initialize this ViewModel.  
/// For mor information on path finding ViewModels see <see cref="PathFindingViewModelBase"/>.  
/// </summary>
public class PathFindingViewModel : PathFindingViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }
    /// <summary>
    /// Corresponding ModelView to this ViewModel used for providing data and services over Model layer of application.
    /// </summary>
    private PFPathFindingModelView _pathFindingMv; 

    /// <summary>
    /// Private collection of accepted points of track selected by user. 
    /// </summary>
    private List<CanvasCoordinate> _acceptedTrackPointList = new();
    /// <summary>
    /// Indicator of the count of accepted track points.
    /// </summary>
    private int AcceptedTrackPointsCount
    {
        get => _acceptedTrackPointsCount;
        set => this.RaiseAndSetIfChanged(ref _acceptedTrackPointsCount, value);
    } 
    private int _acceptedTrackPointsCount;
    /// <summary>
    /// Constructs path finding ViewModel.
    /// 
    /// It initialize all reactive constructs and creates various reactions to them.  
    /// It also calls <c>WhenActivated</c> method which calls inserted lambda expression on ViewModels activation. This activation secures load of map graphics.  
    /// </summary>
    /// <param name="pathFindingMv">Corresponding ModelView to this ViewModel</param>
    public PathFindingViewModel(PFPathFindingModelView pathFindingMv)
    {
        _pathFindingMv = pathFindingMv;
        Activator = new ViewModelActivator();

        IObservable<bool> isAcceptingTrack = this.WhenAnyValue(x => x.IsAcceptingTrack);
        IObservable<bool> isShowingPathReport = this.WhenAnyValue(x => x.IsShowingPathReport);
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
                        report => { if (!cancellationToken.IsCancellationRequested) LastReport = report; });
                    return await _pathFindingMv.FindPathAsync(_acceptedTrackPointList, searchingProgress, cancellationToken);
                })
                .TakeUntil(CancelSearchCommand)
            , isAcceptingTrack.CombineLatest(isMoreThanOnePointAccepted, (x,y) => x && y)); 
        
        CancelSearchCommand = ReactiveCommand.Create(() => { }, FindPathCommand.IsExecuting);

        CleanUpPathReportCommand = ReactiveCommand.Create(() =>
        {
            IsShowingPathReport = false;
            LastReport = null;
        }, isShowingPathReport);
        
        AddTrackPointCommand = ReactiveCommand.CreateFromTask(async ((CanvasCoordinate position, int? index) parameters) =>
        {
            if(parameters.index is null) _acceptedTrackPointList.Add(parameters.position);
            else _acceptedTrackPointList.Insert(parameters.index.Value, parameters.position);
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
            CancelSearchCommand.Execute().Subscribe();
            pathFindingMv.OnClosed();
        });
        
        FindPathCommand.Subscribe(pathReport =>
        {
            IsShowingPathReport = true;
            LastReport = pathReport;
        });

        CancelSearchCommand.Subscribe( _ =>
        {
            IsAcceptingTrack = true; // Showing path report state is skipped due cancellation of path finding.
            LastReport = null; // Cleaning of last report.
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

        _lastReportGraphicsSource = this.WhenAnyValue(x => x.LastReport,
                lastReport => lastReport?.GraphicsSource)
            .ToProperty(this, nameof(LastReportGraphicsSource));
        _scaledGraphicsWidth = this.WhenAnyValue(x => x.GraphicsWidth,
                x => x.GraphicsScale,
                (width, scale) => (int)(width * scale))
            .ToProperty(this, nameof(ScaledGraphicsWidth));
        _scaledGraphicsHeight = this.WhenAnyValue(x => x.GraphicsHeight,
                x => x.GraphicsScale,
                (height, scale) => (int)(height * scale))
            .ToProperty(this, nameof(ScaledGraphicsHeight));
        
        this.WhenActivated( disposables =>
        {
            GetMapGraphicsCommand.Execute().Subscribe(mapGraphicsSource =>
            {
                MapGraphicsSource = mapGraphicsSource;
                GraphicsWidth = MapGraphicsSource.GraphicsWidth;
                GraphicsHeight = MapGraphicsSource.GraphicsHeight;
            }).DisposeWith(disposables);
        });

    }

    /// <summary>
    /// Indicator whether path finding process is in state when found paths report is shown.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public bool IsShowingPathReport
    {
        get => _isShowingPathReport;
        set => this.RaiseAndSetIfChanged(ref _isShowingPathReport, value);
    }
    private bool _isShowingPathReport = false;

    
    /// <summary>
    /// Indicator whether path finding process is in state in which accepting of track from user is enabled.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public bool IsAcceptingTrack
    {
        get => _isAcceptingTrack;
        set => this.RaiseAndSetIfChanged(ref _isAcceptingTrack, value);
    }
    private bool _isAcceptingTrack = true;

    /// <summary>
    /// Source of maps graphics. It is used for rendering maps objects on canvas for user.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public GraphicsSourceViewModel? MapGraphicsSource
    {
        get => _mapGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _mapGraphicsSource, value);
    }
    private GraphicsSourceViewModel? _mapGraphicsSource;

    /// <summary>
    /// Source of by user selected tracks graphics. It is used for rendering tracks objects for user.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public GraphicsSourceViewModel? TrackGraphicsSource
    {
        get => _trackGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _trackGraphicsSource, value);
    }
    private GraphicsSourceViewModel? _trackGraphicsSource;

    /// <summary>
    /// Most recent reported searching state or found path.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public GraphicsContainingDataViewModel? LastReport
    {
        get => _lastReport;
        set => this.RaiseAndSetIfChanged(ref _lastReport, value);
    }
    private GraphicsContainingDataViewModel? _lastReport;

    /// <summary>
    /// Graphics included in most recent report of searching state or found path.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public GraphicsSourceViewModel? LastReportGraphicsSource => _lastReportGraphicsSource.Value;
    private ObservableAsPropertyHelper<GraphicsSourceViewModel?> _lastReportGraphicsSource;

    /// <summary>
    /// Width of maps graphics.
    /// 
    /// Set to be equal to maps graphics source. In the end all graphics sources in this ViewModel should have the same area which respond to area of maps graphics.  
    /// It raises notification about change of its value.  
    /// </summary>
    public int GraphicsWidth
    {
        get => _graphicsWidth;
        set => this.RaiseAndSetIfChanged(ref _graphicsWidth, value);
    }
    private int _graphicsWidth;

    /// <summary>
    /// Height of graphics.
    ///
    /// Set to be equal to maps graphics source. In the end all graphics sources in this ViewModel should have the same area which respond to area of maps graphics.  
    /// It raises notification about change of its value.  
    /// </summary>
    public int GraphicsHeight
    {
        get => _graphicsHeight;
        set => this.RaiseAndSetIfChanged(ref _graphicsHeight, value);
    }
    private int _graphicsHeight;

    /// <summary>
    /// Current scale of rendered graphics.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public float GraphicsScale
    {
        get => _graphicsScale;
        set => this.RaiseAndSetIfChanged(ref _graphicsScale, value);
    }
    private float _graphicsScale = 1;

    /// <summary>
    /// Value of scaled graphics width computed from properties <c>GraphicsWidth</c> and <c>GraphicsScale</c>.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public int ScaledGraphicsWidth => _scaledGraphicsWidth.Value;
    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsWidth;

    /// <summary>
    /// Value of scaled graphics height computed from properties <c>GraphicsHeight</c> and <c>GraphicsScale</c>.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public int ScaledGraphicsHeight => _scaledGraphicsHeight.Value;
    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsHeight;

    /// <summary>
    /// Property which can include information about any problem with rendering of graphics which can be shown to user.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public string? GraphicsProblemText
    {
        get => _graphicsProblemText;
        set => this.RaiseAndSetIfChanged(ref _graphicsProblemText, value);
    }
    private string? _graphicsProblemText;
    
    /// <summary>
    /// Retrieves ground map graphics source from ModelView.
    /// 
    /// Graphics source will be returned immediately but in the background can run asynchronous process that will concurrently generate graphic objects and fill the source with them.  
    /// </summary>
    public ReactiveCommand<Unit, GraphicsSourceViewModel> GetMapGraphicsCommand { get; }
    /// <summary>
    /// Reactive command for executing of path finding mechanism.
    /// 
    /// It can be executed only when is application in state of accepting track and when more then one track point is selected.  
    /// At first it sets <c>IsAcceptingTrack</c> property to false indicating that user can no longer change input track.  
    /// Then instance of <c>Progress{T}"</c> type is created for handling of searching reports from path finding.  
    /// In the end is path finding ModelView asked for asynchronous execution of path finding itself.  
    /// Result of execution is returned for anyone who would care about it.  
    /// Commands execution will take until cancellation command is executed or when is ViewModel informed that Windows closed event was fired by executing <c>OnClosedCommand</c>.  
    /// </summary>
    public ReactiveCommand<Unit, PathReportViewModel?> FindPathCommand { get; }
    /// <summary>
    /// Reactive command for cancelling of path finding.
    /// 
    /// Its is enabled only when <c>FindPathCommand</c> is executing.  
    /// </summary>
    public ReactiveCommand<Unit, Unit> CancelSearchCommand { get; }
    /// <summary>
    /// Reactive command which cleans up report of found path.
    /// 
    /// It is enabled only when application is in state of showing found paths report.  
    /// </summary>
    public ReactiveCommand<Unit, Unit> CleanUpPathReportCommand { get; }
    /// <summary>
    /// Reactive command which adds track point to list of selected track points by user on specified position. If specified position is null, track point is added at the end of list.
    /// 
    /// It is enabled only when application is in state of accepting track from user.  
    /// </summary>
    public ReactiveCommand<(CanvasCoordinate, int?), GraphicsSourceViewModel> AddTrackPointCommand { get; }
    /// <summary>
    /// Reactive command which removes track point from list of selected track points at specified position.
    /// 
    /// If specified position is null, the most recently added point is removed.  
    /// Command is enabled only when application is in state of accepting track and when at least one point of track is already selected.  
    /// </summary>
    public ReactiveCommand<int?, GraphicsSourceViewModel> RemoveTrackPointCommand { get; }
    /// <summary>
    /// Reactive command which zooms whole graphics shown to user by adjusting <c>GraphicsScale</c> property.
    /// 
    /// Graphics scale is adjusted by inputted value.  
    /// </summary>
    public ReactiveCommand<float, Unit> ZoomCommand { get; }
    /// <summary>
    /// Reactive command which un-zooms whole graphics shown to user by adjusting <c>GraphicsScale</c> property.
    /// 
    /// Graphics scale is adjusted by inputted value.  
    /// </summary>
    public ReactiveCommand<float, Unit> UnZoomCommand { get; }
    /// <summary>
    /// Reactive command for exiting of path finding session.
    /// 
    /// Path finding session ViewModel subscribes on this command and on its execution initiates closing of path finding sessions main Window.  
    /// </summary>
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    /// <summary>
    /// Reactive command for handling of main sessions window closed event.
    /// 
    /// It is executed by <c>PathFindingSessionViewModel</c> when such event occurs.  
    /// It informs corresponding ModelView about this fact so it could process it.  
    /// </summary>
    public sealed override ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
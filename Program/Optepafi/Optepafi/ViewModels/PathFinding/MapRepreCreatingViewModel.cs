using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data.Reports;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

/// <summary>
/// ViewModel which is responsible for control over map representation creation for path finding session.
/// 
/// Its tasks include:
/// 
/// - providing check of prerequisites for map representation creation (elevation data dependency, ...).
/// - provides commands for cancelling of map representations creation
/// - when all prerequisites are checked and resolved, it executes map representation creation
/// - provides ViewModel of map repre. creation report. It contains percentage progress of creation.
///
/// For more information on path finding ViewModels see <see cref="PathFindingViewModelBase"/>.  
/// </summary>
public class MapRepreCreatingViewModel : PathFindingViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }
    
    /// <summary>
    /// Corresponding ModelView to this ViewModel used for providing data and services over Model layer of application.
    /// </summary>
    private PFMapRepreCreatingModelView _mapRepreCreatingMv;
    
    /// <summary>
    /// Constructs map representation creation ViewModel.
    /// 
    /// It initialize all reactive constructs and creates various reactions to them.  
    /// It also calls <c>WhenActivated</c> method which calls prerequisites check method right after ViewModels activation.  
    /// </summary>
    /// <param name="mapRepreCreatingMv">Corresponding ModelView to this ViewModel.</param>
    /// <param name="mainSettingsProvider">Provider of main settings.</param>
    public MapRepreCreatingViewModel(PFMapRepreCreatingModelView mapRepreCreatingMv, MainSettingsViewModel.Provider mainSettingsProvider)
    {
        _mapRepreCreatingMv = mapRepreCreatingMv;
        Activator = new ViewModelActivator();
        MainSettingsProvider = mainSettingsProvider;
        
        IObservable<bool> isAwaitingResolution = this.WhenAnyValue(
            x => x.IsAwaitingElevDataAbsenceResolution,
            x => x.IsAwaitingMapNotSupportedByElevDataDistributionResolution,
            (a,b) => a || b);
        
        CheckPrerequisitesCommand = ReactiveCommand.CreateFromTask(async ct =>
        {
            CurrentProcedureInfoText = "Elevation data requirements checking"; //TODO: localize
            DialogText = null;
            var result = await Task.Run(() => _mapRepreCreatingMv.CheckMapRequirementsForElevData(ct));
            if (ct.IsCancellationRequested) return PrerequisitiesCheckResult.Canceled; 
            switch (result)
            {
                case PFMapRepreCreatingModelView.ElevDataPrerequisiteCheckResult.ElevDataForMapNotPresent:
                    return PrerequisitiesCheckResult.ElevDataAbsent;
                case PFMapRepreCreatingModelView.ElevDataPrerequisiteCheckResult.MapNotSupportedByElevDataDistribution:
                    return PrerequisitiesCheckResult.MapNotSupportedByElevDataDistribution;
                case PFMapRepreCreatingModelView.ElevDataPrerequisiteCheckResult.Cancelled:
                    return PrerequisitiesCheckResult.Canceled;
            }
            return PrerequisitiesCheckResult.Ok;
        });

        
        CreateMapRepreCommand = ReactiveCommand.CreateFromObservable(
            () => Observable
                .StartAsync(async ct =>
                {
                    CurrentProcedureInfoText = null; //TODO: localize
                    DialogText = null;
                    IProgress<MapRepreConstructionReportViewModel> mapCreationProgress = 
                        new Progress<MapRepreConstructionReportViewModel>(report => PercentageMapRepreCreationProgress = report.PercentProgress);
                    IProgress<string> progressInfo = new Progress<string>(info => CurrentProcedureInfoText = info);
                    await _mapRepreCreatingMv.CreateMapRepreAsync(progressInfo, mapCreationProgress, ct);
                    return true;
                })
                .TakeUntil(CancelMapRepreCreationCommand));
        
        CancelMapRepreCreationCommand = ReactiveCommand.Create(() => false, CreateMapRepreCommand.IsExecuting);
        
        
        CheckPrerequisitesCommand.Subscribe(prereqCheckResult =>
        {
            switch (prereqCheckResult)
            {
                case PrerequisitiesCheckResult.Ok:
                    CreateMapRepreCommand.Execute().Subscribe();
                    break;
                case PrerequisitiesCheckResult.ElevDataAbsent:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data for given map are not awailable.\n " +
                                 "Return to elevation data settings and download \n" +
                                 "corresponing data for chosen map \n" +
                                 "or use another elevation data source."; //TODO: localize
                    break;
                case PrerequisitiesCheckResult.MapNotSupportedByElevDataDistribution:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data can not be retrieved for given map.\n" +
                                 "Please, choose different map or elevation data \n" +
                                 "source and try again."; //TODO: localize
                    break;
            }
        });

        _isMapRepreCreateCommandExecuting = this.WhenAnyObservable(
            x => x.CreateMapRepreCommand.IsExecuting)
            .ToProperty(this, nameof(IsMapRepreCreateCommandExecuting));
        _isAwaitingElevDataAbsenceResolution = this.WhenAnyObservable(
                x => x.CreateMapRepreCommand.IsExecuting,
                x => x.CheckPrerequisitesCommand.IsExecuting,
                x => x.CheckPrerequisitesCommand,
                (isMapRepreCreating, isPrereqChecking, prereqCheckResult) => 
                    !isMapRepreCreating && !isPrereqChecking && prereqCheckResult is PrerequisitiesCheckResult.ElevDataAbsent)
            .ToProperty(this, nameof(IsAwaitingElevDataAbsenceResolution));
        _isAwaitingMapNotSupportedByElevDataDistributionResolution = this.WhenAnyObservable(
                x => x.CheckPrerequisitesCommand.IsExecuting,
                x => x.CreateMapRepreCommand.IsExecuting,
                x => x.CheckPrerequisitesCommand,
                (isMapRepreCreating, isPrereqChecking, prereqCheckResult) => !isMapRepreCreating && !isPrereqChecking && prereqCheckResult is PrerequisitiesCheckResult.MapNotSupportedByElevDataDistribution)
            .ToProperty(this, nameof(IsAwaitingMapNotSupportedByElevDataDistributionResolution));
        
        ReturnCommand = ReactiveCommand.Create(() => false, isAwaitingResolution);
        
        this.WhenActivated(disposalbes =>
        {
            CheckPrerequisitesCommand.Execute().Subscribe().DisposeWith(disposalbes);
        });
    }

    public MainSettingsViewModel.Provider MainSettingsProvider { get; }
    
    /// <summary>
    /// Percentage progress of map representations creation indicator.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public float PercentageMapRepreCreationProgress
    {
        get => _percentageMapRepreCreationProgress;
        set => this.RaiseAndSetIfChanged(ref _percentageMapRepreCreationProgress, value);
    }
    private float _percentageMapRepreCreationProgress;

    /// <summary>
    /// Text which can be shown to user when his action is needed.
    /// 
    /// Used for resolving of prerequisites check problems.  
    /// It raises notification about change of its value.  
    /// </summary>
    public string? DialogText
    {
        get => _dialogText;
        set => this.RaiseAndSetIfChanged(ref _dialogText, value);
    }
    private string? _dialogText;

    /// <summary>
    /// Text of information about currently executed procedure.
    ///
    /// It can contain simple information about what process is currently running at background.  
    /// It raises notification about change of its value.  
    /// </summary>
    public string? CurrentProcedureInfoText
    {
        get => _currentProcedureInfoText;
        set => this.RaiseAndSetIfChanged(ref _currentProcedureInfoText, value);
    }
    private string? _currentProcedureInfoText;
    
    /// <summary>
    /// Indicates state of map repre. creation process when absence of elevation data for creation should be resolved.
    ///
    /// In this state application waits for input from user.  
    /// It raises notification about change of its value.  
    /// </summary>
    public bool IsAwaitingElevDataAbsenceResolution => _isAwaitingElevDataAbsenceResolution.Value;
    private ObservableAsPropertyHelper<bool> _isAwaitingElevDataAbsenceResolution;
    /// <summary>
    /// Indicates state of map repre. creation process when problem with distribution not supporting provided map type should be resolved.
    ///
    /// In this state application waits for input from user.  
    /// It raises notification about change of its value.  
    /// </summary>
    public bool IsAwaitingMapNotSupportedByElevDataDistributionResolution => _isAwaitingMapNotSupportedByElevDataDistributionResolution.Value;
    private ObservableAsPropertyHelper<bool> _isAwaitingMapNotSupportedByElevDataDistributionResolution;
    /// <summary>
    /// Indicates that map representation creation takes place.
    /// 
    /// It raises notification about change of its value.
    /// </summary>
    public bool IsMapRepreCreateCommandExecuting { get => _isMapRepreCreateCommandExecuting.Value; }
    private ObservableAsPropertyHelper<bool> _isMapRepreCreateCommandExecuting;
    
    /// <summary>
    /// Enumeration of every prerequisite check results.
    /// </summary>
    public enum PrerequisitiesCheckResult {Ok, ElevDataAbsent, MapNotSupportedByElevDataDistribution, Canceled}
    
    /// <summary>
    /// Reactive command whether all prerequisites for map representation creation are satisfied.
    /// 
    /// It runs asynchronously check for elevation data requirements of map repre. creation.  
    /// Based on result reports corresponding <c>PrerequisitesCheckResult</c>.  
    /// This command can be expanded by other prerequisites checks which could in future occur.  
    /// </summary>
    public ReactiveCommand<Unit, PrerequisitiesCheckResult> CheckPrerequisitesCommand { get; }
    
    /// <summary>
    /// Reactive command for executing of maps creation.
    /// 
    /// This command is called from <c>CheckPrerequisitesCommand</c>s subscription upon positive prerequisites check.  
    /// It creates instance of map repre. creation progress for tracking of reports about creation process and asynchronously asks ModelView for execution of map repre. creation.  
    /// In the end it returns true for indication that map representation creation was successful.  
    /// Commands execution will take until it is done or cancellation command is executed.  
    /// </summary>
    public ReactiveCommand<Unit, bool> CreateMapRepreCommand { get; }
    /// <summary>
    /// Reactive command for cancelling of map representations creation.
    /// 
    /// It returns false for indication that map representation was not created.  
    /// It can be executed only when <c>CreateMapRepreCommand</c> is executing.  
    /// </summary>
    public ReactiveCommand<Unit, bool> CancelMapRepreCreationCommand { get; }
    /// <summary>
    /// Reactive command for unsuccessful returning from map representation creation ViewModel.
    /// 
    /// It returns false for indication that map representation was not created.  
    /// It can be executed only when resolution with some prerequisite is expected.  
    /// It represents ability for user to exit map repre. creation part of path finding session and return to settings.  
    /// </summary>
    public ReactiveCommand<Unit, bool> ReturnCommand { get; }
}
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Optepafi.Models.MapRepreMan;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class MapRepreCreatingWindowViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }
    
    private PFMapRepreCreatingModelView _mapRepreCreatingMv;
    public MapRepreCreatingWindowViewModel(PFMapRepreCreatingModelView mapRepreCreatingMv)
    {
        _mapRepreCreatingMv = mapRepreCreatingMv;
        Activator = new ViewModelActivator();
        
        CheckPrerequisitiesCommand = ReactiveCommand.CreateFromTask(async ct =>
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

        ReturnCommand = ReactiveCommand.Create(() => false);
        
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
        
        this.WhenActivated(disposalbes =>
        {
            CheckPrerequisitiesCommand.Execute().Subscribe().DisposeWith(disposalbes);
        });
        
        CheckPrerequisitiesCommand.Subscribe(prereqCheckResult =>
        {
            switch (prereqCheckResult)
            {
                case PrerequisitiesCheckResult.Ok:
                    CreateMapRepreCommand.Execute().Subscribe();
                    break;
                case PrerequisitiesCheckResult.ElevDataAbsent:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data for given map are not awailable.\n " +
                                 "Return to elevation data settings and download corresponing data for chosen map \n" +
                                 "or use another elevation data source."; //TODO: localize
                    break;
                case PrerequisitiesCheckResult.MapNotSupportedByElevDataDistribution:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data can not be retrieved for given map.\n" +
                                 "Please, choose different map or elevation data source and try again."; //TODO: localize
                    break;
                case PrerequisitiesCheckResult.Canceled:
                    CurrentProcedureInfoText = "Creation canceled"; //TODO: localize
                    DialogText = null;
                    break;
            }
        });

        _isMapRepreCreateCommandExecuting = CreateMapRepreCommand.IsExecuting
            .ToProperty(this, nameof(IsMapRepreCreateCommandExecuting));
        _isAwaitingElevDataAbsenceResolution = this.WhenAnyObservable(
                x => x.CreateMapRepreCommand.IsExecuting,
                x => x.CheckPrerequisitiesCommand.IsExecuting,
                x => x.CheckPrerequisitiesCommand,
                (isMapRepreCreating, isPrereqChecking, prereqCheckResult) => 
                    !isMapRepreCreating && !isPrereqChecking && prereqCheckResult is PrerequisitiesCheckResult.ElevDataAbsent)
            .ToProperty(this, nameof(IsAwaitingElevDataAbsenceResolution));
        _isAwaitingMapNotSupportedByElevDataDistributionResolution = this.WhenAnyObservable(
                x => x.CheckPrerequisitiesCommand.IsExecuting,
                x => x.CreateMapRepreCommand.IsExecuting,
                x => x.CheckPrerequisitiesCommand,
                (isMapRepreCreating, isPrereqChecking, prereqCheckResult) => !isMapRepreCreating && !isPrereqChecking && prereqCheckResult is PrerequisitiesCheckResult.MapNotSupportedByElevDataDistribution)
            .ToProperty(this, nameof(IsAwaitingMapNotSupportedByElevDataDistributionResolution));
        
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    
    private float _percentageMapRepreCreationProgress;
    public float PercentageMapRepreCreationProgress
    {
        get => _percentageMapRepreCreationProgress;
        set => this.RaiseAndSetIfChanged(ref _percentageMapRepreCreationProgress, value);
    }

    private string? _dialogText = null;
    public string? DialogText
    {
        get => _dialogText;
        set => this.RaiseAndSetIfChanged(ref _dialogText, value);
    }

    private string? _currentProcedureInfoText = null;
    public string? CurrentProcedureInfoText
    {
        get => _currentProcedureInfoText;
        set => this.RaiseAndSetIfChanged(ref _currentProcedureInfoText, value);
    }
    
    private ObservableAsPropertyHelper<bool> _isAwaitingElevDataAbsenceResolution;
    public bool IsAwaitingElevDataAbsenceResolution => _isAwaitingElevDataAbsenceResolution.Value;
    private ObservableAsPropertyHelper<bool> _isAwaitingMapNotSupportedByElevDataDistributionResolution;
    public bool IsAwaitingMapNotSupportedByElevDataDistributionResolution => _isAwaitingMapNotSupportedByElevDataDistributionResolution.Value;
    private ObservableAsPropertyHelper<bool> _isMapRepreCreateCommandExecuting;
    public bool IsMapRepreCreateCommandExecuting { get => _isMapRepreCreateCommandExecuting.Value; }
    
    public enum PrerequisitiesCheckResult {Ok, ElevDataAbsent, MapNotSupportedByElevDataDistribution, Canceled}
    public ReactiveCommand<Unit, PrerequisitiesCheckResult> CheckPrerequisitiesCommand { get; }
    public ReactiveCommand<Unit, bool> CreateMapRepreCommand { get; }
    public ReactiveCommand<Unit, bool> CancelMapRepreCreationCommand { get; }
    public ReactiveCommand<Unit, bool> ReturnCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
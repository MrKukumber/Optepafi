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
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class MapRepreCreatingWindowViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }
    
    private PFMapRepreCreatingModelView _mapRepreCreatingMv;
    public MapRepreCreatingWindowViewModel(PFMapRepreCreatingModelView mapRepreCreatingMv)
    {
        Activator = new ViewModelActivator();
        _mapRepreCreatingMv = mapRepreCreatingMv;
        
        CheckPrerequisitiesCommand = ReactiveCommand.CreateFromTask(async ct =>
        {
            CurrentProcedureInfoText = "Elevation data requirements checking"; //TODO: localize
            DialogText = null;
            var result = await Task.Run(() => _mapRepreCreatingMv.CheckMapRequirementForElevData());
            if (ct.IsCancellationRequested)
            {
                PrereqCheckResult = PrerequisitiesCheckResult.Canceled;
                return;
            }
            switch (result)
            {
                case PFMapRepreCreatingModelView.ElevDataPrerequisityResult.ElevDataForMapNotPresent:
                    PrereqCheckResult =  PrerequisitiesCheckResult.ElevDataAbsent;
                    return;
                case PFMapRepreCreatingModelView.ElevDataPrerequisityResult.MapNotSupportedByElevDataType:
                    PrereqCheckResult =  PrerequisitiesCheckResult.MapNotSupportedByElevDataType;
                    return;
            }
            PrereqCheckResult = PrerequisitiesCheckResult.Ok;
        });

        
        ReturnCommand = ReactiveCommand.Create(() =>
        {
            // IsPossibleToContinue = false;
            return false;
        });

        CreateMapRepreCommand = ReactiveCommand.CreateFromObservable(
            () => Observable
                .StartAsync(async ct =>
                {
                    CurrentProcedureInfoText = "Creating map representation."; //TODO: localize
                    DialogText = null;
                    IProgress<MapRepreCreationReport> progress = new Progress<MapRepreCreationReport>(HandleMapCreationProgres);
                    await _mapRepreCreatingMv.CreateMapRepreAsync(progress, ct);
                    if (ct.IsCancellationRequested)
                    {
                        _mapRepreCreatingMv.CleanMapRepre();
                        return false;
                    }
                    return true;
                })
                .TakeUntil(CancelMapRepreCreationCommand));
        
        CancelMapRepreCreationCommand = ReactiveCommand.Create(() => {}, CreateMapRepreCommand.IsExecuting);

        
        // ReturnValueSet = this.WhenAnyValue(x => x.IsPossibleToContinue);
        
        this.WhenActivated(disposalbes =>
        {
            CheckPrerequisitiesCommand.Execute().Subscribe().DisposeWith(disposalbes);
        });
        
        this.WhenAnyValue(x => x.PrereqCheckResult).Subscribe(prereqCheckResult =>
        {
            switch (prereqCheckResult)
            {
                case PrerequisitiesCheckResult.Ok:
                    CreateMapRepreCommand.Execute();
                    break;
                case PrerequisitiesCheckResult.ElevDataAbsent:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data for given map are not awailable.\n " +
                                 "Return to elevation data settings and download corresponing data for chosen map \n" +
                                 "or use another elevation data source."; //TODO: localize
                    break;
                case PrerequisitiesCheckResult.MapNotSupportedByElevDataType:
                    CurrentProcedureInfoText = "Elevation data problem"; //TODO: localize
                    DialogText = "Elevation data can not be retrieved for given map.\n " +
                                 "Please, choose different map or elevation data source and try again."; //TODO: localize
                    break;
            }
        });

        _isMapRepreCreateCommandExecuting = CreateMapRepreCommand.IsExecuting
            .ToProperty(this, nameof(IsMapRepreCreateCommandExecuting));
        _isPrerequisitiesCheckExecuting = CheckPrerequisitiesCommand.IsExecuting
            .ToProperty(this, nameof(IsPrerequisitesCheckExecuting));
        
        // CreateMapRepreCommand.Subscribe(cancellationToken =>
        // {
            // DialogText = null;
            // if (cancellationToken.IsCancellationRequested)
            // {
                // _mapRepreCreatingMv.CleanCreatedMapRepre();
                // IsPossibleToContinue = false;
            // }
            // else
            // {
                // IsPossibleToContinue = true;
            // }
        // });

    }

    // public IObservable<bool> ReturnValueSet { get; }

    // private bool _isPossibleToContinue;
    // public bool IsPossibleToContinue
    // {
        // get => _isPossibleToContinue;
        // set => this.RaiseAndSetIfChanged(ref _isPossibleToContinue, value);
    // }
    
    private void HandleMapCreationProgres(MapRepreCreationReport report)
    {
        PercentageMapRepreCreationProgress = report.PercentualProgress;
    }

    private float _percentageMapRepreCreationProgress;
    public float PercentageMapRepreCreationProgress
    {
        get => _percentageMapRepreCreationProgress;
        set => this.RaiseAndSetIfChanged(ref _percentageMapRepreCreationProgress, value);
    }

    private ObservableAsPropertyHelper<bool> _isMapRepreCreateCommandExecuting;
    public bool IsMapRepreCreateCommandExecuting { get => _isMapRepreCreateCommandExecuting.Value; }
    private ObservableAsPropertyHelper<bool> _isPrerequisitiesCheckExecuting;
    public bool IsPrerequisitesCheckExecuting { get => _isPrerequisitiesCheckExecuting.Value; }

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
    
    public enum PrerequisitiesCheckResult {Ok, ElevDataAbsent, MapNotSupportedByElevDataType, Canceled}
    private PrerequisitiesCheckResult _prereqCheckResult;
    public PrerequisitiesCheckResult PrereqCheckResult
    {
        get => _prereqCheckResult;
        set => this.RaiseAndSetIfChanged(ref _prereqCheckResult, value);
    }
    
    public ReactiveCommand<Unit, Unit> CheckPrerequisitiesCommand { get; }
    public ReactiveCommand<Unit, bool> CreateMapRepreCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelMapRepreCreationCommand { get; }
    public ReactiveCommand<Unit, bool> ReturnCommand { get; }

    public static FuncValueConverter<PrerequisitiesCheckResult, bool> AreElevDataAbsent { get; } =
        new(result => result is PrerequisitiesCheckResult.ElevDataAbsent);
    public static FuncValueConverter<PrerequisitiesCheckResult, bool> IsMapNotSupportedByElevDataType { get; } =
        new(result => result is PrerequisitiesCheckResult.MapNotSupportedByElevDataType);
}
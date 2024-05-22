using System;
using System.Reactive;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.Main;
using Optepafi.Views.Main;
using ReactiveUI;
using PathFindingSessionModelView = Optepafi.ModelViews.PathFinding.PathFindingSessionModelView;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSessionViewModel : SessionViewModel
{
    private ViewModelBase _currentViewModel;
    public PathFindingViewModel PathFinding { get; }
    public PathFindingSettingsViewModel PathFindingSettings { get; }
    // public RelevanceFeedbackViewModel RelevanceFeedback { get; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    public PathFindingSessionViewModel(PathFindingSessionModelView pathFindingSessionMv, MainSettingsModelView.Provider mainSettingsMvProvider)
    {
        PathFindingSettings = new PathFindingSettingsViewModel(pathFindingSessionMv.Settings, mainSettingsMvProvider, pathFindingSessionMv.MapRepreCreating);
        // RelevanceFeedback = new RelevanceFeedbackViewModel(pathFindingSessionMv.RelevanceFeedback);
        PathFinding = new PathFindingViewModel(pathFindingSessionMv.PathFinding);
        CurrentViewModel = PathFindingSettings;

        this.WhenAnyObservable(x => x.PathFindingSettings.ProceedTroughMapRepreCreationCommand)
            .Subscribe(whereToContinue => 
            {
                switch (whereToContinue)
                {
                    case PathFindingSettingsViewModel.WhereToProceed.Settings:
                        break;
                    case PathFindingSettingsViewModel.WhereToProceed.PathFinding:
                        CurrentViewModel = PathFinding;
                        break;
                }
            });
        
        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
    
}
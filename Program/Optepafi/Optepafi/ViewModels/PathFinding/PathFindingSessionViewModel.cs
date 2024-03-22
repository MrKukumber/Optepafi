using System;
using System.Reactive;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.Main;
using Optepafi.Views.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSessionViewModel : SessionViewModel
{
    private ViewModelBase _currentViewModel;
    public PathFindingViewModel PathFinding { get; }
    public PathFindingSettingsViewModel PathFindingSettings { get; }
    public RelevanceFeedbackViewModel RelevanceFeedback { get; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    public PathFindingSessionViewModel(MainParamsManagingModelView mainParamsManagingMv, PathFindingSessionModelView pathFindingSessionMv)
    {
        PathFinding = new PathFindingViewModel(pathFindingSessionMv.PathFinding);
        PathFindingSettings = new PathFindingSettingsViewModel(pathFindingSessionMv.Settings, mainParamsManagingMv);
        RelevanceFeedback = new RelevanceFeedbackViewModel(pathFindingSessionMv.RelevanceFeedback);
        CurrentViewModel = PathFindingSettings;

        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return ClosingRecommendation.CanClose;
            //TODO: return correct recommendation for closing the window
        });
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    
    public enum ClosingRecommendation { CanClose }
    
    public ReactiveCommand<Unit, ClosingRecommendation> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
    
}
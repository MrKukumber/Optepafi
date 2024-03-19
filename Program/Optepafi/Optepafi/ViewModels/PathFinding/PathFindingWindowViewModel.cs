using System;
using System.Reactive;
using Optepafi.ViewModels.Main;
using Optepafi.Views.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingWindowViewModel : SessionViewModel
{
    private ViewModelBase _currentViewModel;
    public PathFindingViewModel PathFinding { get; }
    public PathFindingSettingsViewModel PathFindingSettings { get; }
    public RelevanceFeedbackViewModel RelevanceFeedback { get; }
    private MainMenuViewModel MainMenu { get; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    public PathFindingWindowViewModel(MainMenuViewModel mainMenu)
    {
        MainMenu = mainMenu;
        PathFinding = new PathFindingViewModel(this);
        PathFindingSettings = new PathFindingSettingsViewModel(this);
        RelevanceFeedback = new RelevanceFeedbackViewModel(this);
        CurrentViewModel = PathFindingSettings;

        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return ClosingRecommendation.CanClose;
            //TODO: return correct recommendation for closing the window
        });
        OnClosedCommand = ReactiveCommand.Create(() =>
        {
            Console.Write("ahoj");
        });
    }
    
    public enum ClosingRecommendation { CanClose }
    
    public ReactiveCommand<Unit, ClosingRecommendation> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
    
}
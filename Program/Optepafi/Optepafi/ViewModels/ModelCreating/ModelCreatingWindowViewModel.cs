using System.Reactive;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingWindowViewModel : SessionViewModel
{
    private ViewModelBase _currentViewModel;
    public ModelCreatingViewModel ModelCreating { get; }
    private MainMenuViewModel MainMenu{ get; }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public ModelCreatingWindowViewModel(MainMenuViewModel mainMenu)
    {
        MainMenu = mainMenu;
        ModelCreating = new ModelCreatingViewModel(this);
        CurrentViewModel = ModelCreating;
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
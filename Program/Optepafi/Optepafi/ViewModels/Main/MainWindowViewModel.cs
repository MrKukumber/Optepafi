using System.Diagnostics;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
{
    private ViewModelBase _currentViewModel;
    public MainMenuViewModel MainMenu { get; }
    public MainSettingsViewModel MainSettings { get; }
    public ElevConfigViewModel ElevConfig { get; }
    public ViewModelActivator Activator { get; }
    public MainWindowViewModel()
    {
        MainMenu = new MainMenuViewModel(this);
        MainSettings = new MainSettingsViewModel(this);
        ElevConfig = new ElevConfigViewModel(this);
        CurrentViewModel = MainMenu;
        Activator = new ViewModelActivator();
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
}
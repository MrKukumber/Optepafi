using System.ComponentModel;
using System.Diagnostics;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    public MainMenuViewModel MainMenu { get; }
    public MainSettingsViewModel MainSettings { get; }
    public ElevConfigViewModel ElevConfig { get; }
    public MainWindowViewModel()
    {
        MainMenu = new MainMenuViewModel(this);
        MainSettings = new MainSettingsViewModel(this);
        ElevConfig = new ElevConfigViewModel(this);
        CurrentViewModel = MainMenu;
        
        
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    

    public void MainWindow_Closing(object sender, CancelEventArgs args)
    {
        
    }

}
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    public ICommand GoToMainMenuCommand { get; }
    public ICommand GoToElevConfigCommand { get; }
    private MainWindowViewModel ParentMainWindow { get; }
    public MainSettingsViewModel(MainWindowViewModel parentMainWindow)
    {
        ParentMainWindow = parentMainWindow;
        GoToMainMenuCommand = ReactiveCommand.Create(() =>
            ParentMainWindow.CurrentViewModel = ParentMainWindow.MainMenu);
        GoToElevConfigCommand = ReactiveCommand.Create(() =>
            ParentMainWindow.CurrentViewModel = ParentMainWindow.ElevConfig);
    }
}
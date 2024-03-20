using System.Reactive;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public MainSettingsViewModel()
    {
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        OpenElevConfigCommand = ReactiveCommand.Create(() => { });
    }
}
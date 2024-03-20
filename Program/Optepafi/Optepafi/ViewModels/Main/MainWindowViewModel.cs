using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.PlatformServices;
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
        MainMenu = new MainMenuViewModel();
        MainSettings = new MainSettingsViewModel();
        ElevConfig = new ElevConfigViewModel();
        CurrentViewModel = MainMenu;

        this.WhenAnyObservable(x => x.MainMenu.GoToSettingsCommand,
                x => x.ElevConfig.ReturnCommand)
            .Subscribe(_ => CurrentViewModel = MainSettings);
        this.WhenAnyObservable(x => x.MainSettings.GoToMainMenuCommand)
            .Subscribe(_ => CurrentViewModel = MainMenu);
        this.WhenAnyObservable(x => x.MainSettings.OpenElevConfigCommand)
            .Subscribe(_ => CurrentViewModel = ElevConfig);
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

}
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.PlatformServices;
using Optepafi.ModelViews.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainWindowViewModel : ViewModelBase
{
    private MainWindowModelView _mainWindowModelView;
    
    private ViewModelBase _currentViewModel;
    public MainMenuViewModel MainMenu { get; }
    public MainSettingsViewModel MainSettings { get; }
    public MainWindowViewModel(MainWindowModelView mainWindowModelView )
    {
        _mainWindowModelView = mainWindowModelView;
        MainMenu = new MainMenuViewModel(_mainWindowModelView.MainSettings);
        MainSettings = new MainSettingsViewModel(_mainWindowModelView.MainSettings);
        CurrentViewModel = MainMenu;

        this.WhenAnyObservable(x => x.MainMenu.GoToSettingsCommand)
            .Subscribe(_ => CurrentViewModel = MainSettings);
        this.WhenAnyObservable(x => x.MainSettings.GoToMainMenuCommand)
            .Subscribe(_ => CurrentViewModel = MainMenu);
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

}
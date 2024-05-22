using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
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
        MainMenu = new MainMenuViewModel(_mainWindowModelView.MainSettings.ProviderOfSettings);
        MainSettings = new MainSettingsViewModel(_mainWindowModelView.MainSettings);
        CurrentViewModel = MainMenu;

        this.WhenAnyObservable(x => x.MainMenu.GoToSettingsCommand)
            .Subscribe(_ => CurrentViewModel = MainSettings);
        this.WhenAnyObservable(x => x.MainSettings.GoToMainMenuCommand)
            .Subscribe(_ => CurrentViewModel = MainMenu);
        
        YesNoInteraction = new Interaction<YesNoDialogWindowViewModel, bool>();
        OnClosingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (MainMenu.Sessions.Count > 0)
            {
                return await YesNoInteraction
                    .Handle(new YesNoDialogWindowViewModel("There are some opend sessions.\nDo you realy wish to exit?", "Yes", "No")); //TODO: localize
            }
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() =>
        {
            _mainWindowModelView.SaveParams();
        });
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public Interaction<YesNoDialogWindowViewModel, bool> YesNoInteraction { get; }
    
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
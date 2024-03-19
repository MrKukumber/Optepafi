using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using ExCSS;
using Optepafi.ViewModels.ModelCreating;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.ModelCreating;
using Optepafi.Views.PathFinding;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainMenuViewModel : ViewModelBase
{
    public ObservableCollection<SessionViewModel> Sessions { get; } = new();
    public MainWindowViewModel ParentMainWindow { get; }
    public MainMenuViewModel(MainWindowViewModel parentMainWindow)
    {
        IObservable<bool> isSessionsCountLessThanEight = Sessions.WhenAnyValue(
            s => s.Count,
             count => count <= 8);
        ParentMainWindow = parentMainWindow;
        GoToSettingsCommand = ReactiveCommand.Create(() =>
        {
           parentMainWindow.CurrentViewModel = parentMainWindow.MainSettings;
        });
        CreatePathFindingSessionCommand = ReactiveCommand.Create(() =>
            {
                var pathFindingSession = new PathFindingWindowViewModel(this);
                Sessions.Add(pathFindingSession);
                pathFindingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(pathFindingSession));
                return pathFindingSession;
            },
            isSessionsCountLessThanEight);
        CreateModelCreatingSessionCommand = ReactiveCommand.Create(() =>
            {
                var modelCreatingSession = new ModelCreatingWindowViewModel(this);
                Sessions.Add(modelCreatingSession);
                modelCreatingSession.WhenActivated(disposables =>
                {
                    modelCreatingSession.WhenAnyObservable(x => x.OnClosedCommand)
                        .Subscribe(_ => Sessions.Remove(modelCreatingSession))
                        .DisposeWith(disposables);
                });
                return modelCreatingSession;
            },
            isSessionsCountLessThanEight);
    }
    
    public ReactiveCommand<Unit,Unit> GoToSettingsCommand { get; }
    public ReactiveCommand<Unit, PathFindingWindowViewModel> CreatePathFindingSessionCommand { get; }
    public ReactiveCommand<Unit, ModelCreatingWindowViewModel> CreateModelCreatingSessionCommand{ get; }
}
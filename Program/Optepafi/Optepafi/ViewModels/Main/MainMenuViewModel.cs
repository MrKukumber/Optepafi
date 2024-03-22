using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using ExCSS;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.ModelCreating;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.ModelCreating;
using Optepafi.Views.PathFinding;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainMenuViewModel : ViewModelBase
{
    public ObservableCollection<SessionViewModel> Sessions { get; } = new();
    public MainMenuViewModel()
    {
        IObservable<bool> isSessionsCountLessThanEight = Sessions.WhenAnyValue(
            s => s.Count,
             count => count <= 8);
        GoToSettingsCommand = ReactiveCommand.Create(() => { });
        CreatePathFindingSessionCommand = ReactiveCommand.Create(() =>
            {
                var pathFindingSession = new PathFindingSessionViewModel(/*TODO: elegantnym sposobom predat hlavneho spravcu parametrov*/, new PathFindingSessionModelView());
                Sessions.Add(pathFindingSession);
                pathFindingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(pathFindingSession));
                return pathFindingSession;
            },
            isSessionsCountLessThanEight);
        CreateModelCreatingSessionCommand = ReactiveCommand.Create(() =>
            {
                var modelCreatingSession = new ModelCreatingSessionViewModel(/*TODO: elegantnym sposobom predat hlavneho spravcu parametrov*/, new ModelCreatingSessionModelView());
                Sessions.Add(modelCreatingSession);
                modelCreatingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(modelCreatingSession));
                return modelCreatingSession;
            },
            isSessionsCountLessThanEight);
    }
    
    public ReactiveCommand<Unit,Unit> GoToSettingsCommand { get; }
    public ReactiveCommand<Unit, PathFindingSessionViewModel> CreatePathFindingSessionCommand { get; }
    public ReactiveCommand<Unit, ModelCreatingSessionViewModel> CreateModelCreatingSessionCommand{ get; }
}
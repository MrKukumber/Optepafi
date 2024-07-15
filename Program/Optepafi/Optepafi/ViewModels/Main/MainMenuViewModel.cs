using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;
using PathFindingSessionModelView = Optepafi.ModelViews.PathFinding.PathFindingSessionModelView;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel which is responsible for control over main menu.
///
/// It is the first ViewModel which corresponding View is shown to user when application starts.  
/// This is special case of ViewModel which does not have its own corresponding ModelView.  
/// All he needs is main settings ModelView provider which it can pass to created sessions.  
/// Its tasks include:
/// 
/// - opportunity for user to visit main settings where he can set parameters which affects whole application
/// - way to create individual sessions up to point when their count reaches maximal value
/// - tracking of opened sessions and their managing
/// </summary>
public class MainMenuViewModel : ViewModelBase
{
    /// <summary>
    /// Provider of main settings. It is used for passing to created sessions so they could access main parameters.
    /// </summary>
    private MainSettingsViewModel.Provider _mainSettingsVmProvider;
    
    /// <summary>
    /// Maximal count of sessions that can be opened at once.
    /// </summary>
    private const int SessionsMaxCount = 8;
    /// <summary>
    /// Collection of currently opened sessions.
    /// </summary>
    public ObservableCollection<SessionViewModel> Sessions { get; } = new();
    /// <summary>
    /// Constructor of main menu ViewModel. It initializes all reactive constructs.
    /// </summary>
    /// <param name="mainSettingsVmProvider">Provider of main settings ModelView which can be passed to created sessions.</param>
    public MainMenuViewModel(MainSettingsViewModel.Provider mainSettingsVmProvider)
    {
        _mainSettingsVmProvider = mainSettingsVmProvider;
        IObservable<bool> isSessionsCountNotMaximal = Sessions.WhenAnyValue(
            s => s.Count,
             count => count < SessionsMaxCount);
        GoToSettingsCommand = ReactiveCommand.Create(() => { });
        CreatePathFindingSessionCommand = ReactiveCommand.Create(() =>
            {
                var pathFindingSession = new PathFindingSessionViewModel(new PathFindingSessionModelView(), _mainSettingsVmProvider);
                Sessions.Add(pathFindingSession);
                pathFindingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(pathFindingSession));
                return pathFindingSession;
            },
            isSessionsCountNotMaximal);
        // CreateModelCreatingSessionCommand = ReactiveCommand.Create(() =>
            // {
                // var modelCreatingSession = new ModelCreatingSessionViewModel(new ModelCreatingSessionModelView());
                // Sessions.Add(modelCreatingSession);
                // modelCreatingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    // .Subscribe(_ => Sessions.Remove(modelCreatingSession));
                // return modelCreatingSession;
            // },
            // isSessionsCountNotMaximal);
    }
    
    /// <summary>
    /// Reactive command which initiate change of currently used ViewModel in <c>MainWindowViewModel</c> to main settings ViewModel.
    /// </summary>
    public ReactiveCommand<Unit,Unit> GoToSettingsCommand { get; }
    /// <summary>
    /// Reactive command for creating and opening path finding session.
    /// 
    /// It initialize and returns new <c>PathFindingSessionViewModel</c> instance which is then handled by View layer (specifically main window) and its corresponding View is shown to user in new window.  
    /// Its OnClosedCommand is subscribed so the closing of session could be processed.  
    /// It is enabled only when count of all opened sessions does not exceeds maximal sessions count.  
    /// </summary>
    public ReactiveCommand<Unit, PathFindingSessionViewModel> CreatePathFindingSessionCommand { get; }
    // public ReactiveCommand<Unit, ModelCreatingSessionViewModel> CreateModelCreatingSessionCommand{ get; }
}
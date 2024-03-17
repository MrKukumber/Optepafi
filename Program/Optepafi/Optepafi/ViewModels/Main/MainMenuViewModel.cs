using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Optepafi.ViewModels.ModelCreating;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.ModelCreating;
using Optepafi.Views.PathFinding;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainMenuViewModel : ViewModelBase
{
    private ObservableCollection<SessionViewModel> Sessions { get; } = new();
    public MainWindowViewModel ParentMainWindow { get; }
    public MainMenuViewModel(MainWindowViewModel parentMainWindow)
    {
        IObservable<bool> isSessionsCountLessThanEight = Sessions.WhenAnyValue(
            s => s.Count,
             count => count <= 8);
        ParentMainWindow = parentMainWindow;
        GoToSettingsCommand = ReactiveCommand.Create(() =>
            parentMainWindow.CurrentViewModel = parentMainWindow.MainSettings);
        CreatePathFindingSessionCommand = ReactiveCommand.Create(() =>
            {
                var pathFindingSession = new PathFindingWindowViewModel(this);
                var newWindow= new PathFindingWindow
                {
                    DataContext = pathFindingSession,
                };
                Sessions.Add(pathFindingSession);
                newWindow.Show();
            },
            isSessionsCountLessThanEight);
        CreateModelCreatingSessionCommand = ReactiveCommand.Create(() =>
            {
                var modelCreatingSession = new ModelCreatingWindowViewModel(this);
                var newWindow= new ModelCreatingWindow
                {
                    DataContext =  modelCreatingSession,
                };
                Sessions.Add(modelCreatingSession);
                newWindow.Show();
            },
            isSessionsCountLessThanEight);
    }
    
    public ICommand GoToSettingsCommand { get; }
    public ICommand CreatePathFindingSessionCommand { get; }
    public ICommand CreateModelCreatingSessionCommand{ get; }
}
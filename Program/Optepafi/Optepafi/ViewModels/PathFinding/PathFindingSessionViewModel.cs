using System;
using System.Reactive;
using System.Runtime.InteropServices.Marshalling;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Main;
using Optepafi.Views.Main;
using ReactiveUI;
using PathFindingSessionModelView = Optepafi.ModelViews.PathFinding.PathFindingSessionModelView;

namespace Optepafi.ViewModels.PathFinding;

/// <summary>
/// Path finding sessions ViewModel. It contains all ViewModels which contribute to effort of delivering path finding mechanism to the user.
/// Additionally it handles closing of path finding sessions main window and therefore closing of session itself.
/// For more information about session ViewModels see <see cref="SessionViewModel"/>
/// </summary>
public class PathFindingSessionViewModel : SessionViewModel
{
    public PathFindingViewModel PathFinding { get; }
    public PathFindingSettingsViewModel PathFindingSettings { get; }
    // public RelevanceFeedbackViewModel RelevanceFeedback { get; }

    /// <summary>
    /// Constructor of session ViewModel It initialize all reactive constructs and creates associated ViewModels.
    /// </summary>
    /// <param name="pathFindingSessionMv">Path finding session ModelView which contains path finding ModelViews which should be assigned to corresponding ViewModels.</param>
    /// <param name="mainSettingsMvProvider">Provider of main settings ModelView. It can be used by some ViewModels/ModelViews for main parameters retrieval.</param>
    public PathFindingSessionViewModel(PathFindingSessionModelView pathFindingSessionMv, MainSettingsModelView.Provider mainSettingsMvProvider)
    {
        PathFindingSettings = new PathFindingSettingsViewModel(pathFindingSessionMv.Settings, mainSettingsMvProvider, pathFindingSessionMv.MapRepreCreating);
        // RelevanceFeedback = new RelevanceFeedbackViewModel(pathFindingSessionMv.RelevanceFeedback);
        PathFinding = new PathFindingViewModel(pathFindingSessionMv.PathFinding);
        CurrentViewModel = PathFindingSettings;

        this.WhenAnyObservable(x => x.PathFindingSettings.ProceedTroughMapRepreCreationCommand)
            .Subscribe(whereToContinue => 
            {
                switch (whereToContinue)
                {
                    case PathFindingSettingsViewModel.WhereToProceed.Settings:
                        break;
                    case PathFindingSettingsViewModel.WhereToProceed.PathFinding:
                        CurrentViewModel = PathFinding;
                        break;
                }
            });
        
        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() =>
        {
            CurrentViewModel.OnClosedCommand?.Execute().Subscribe();
        });
    }

    /// <summary>
    /// Property which contains currently used ViewModel.
    /// It raises notification about change of its value.
    /// </summary>
    public PathFindingViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    private PathFindingViewModelBase _currentViewModel;
    
    /// <summary>
    /// Command which is executed by View when Windows closing event takes place. It allows session ViewModel react to this event.
    /// </summary>
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    /// <summary>
    /// Command which is executed by View when Windows closed event takes place. It allows session ViewModel to react to this event.
    /// </summary>
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
    
}
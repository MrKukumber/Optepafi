using System.Reactive;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingSessionViewModel : SessionViewModel
{
    private ViewModelBase _currentViewModel;
    public ModelCreatingViewModel ModelCreating { get; }
    public ModelCreatingSettingsViewModel ModelCreatingSettings { get; }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public ModelCreatingSessionViewModel(MainParamsManagingModelView mainParamsManaging, ModelCreatingSessionModelView modelCreatingSession)
    {
        ModelCreating = new ModelCreatingViewModel(modelCreatingSession.ModelCreating);
        ModelCreatingSettings = new ModelCreatingSettingsViewModel(modelCreatingSession.Settings, mainParamsManaging);
        CurrentViewModel = ModelCreatingSettings;
        
        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return ClosingRecommendation.CanClose;
            //TODO: return correct recommendation for closing the window
        });
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    
    public enum ClosingRecommendation { CanClose }
    public ReactiveCommand<Unit, ClosingRecommendation> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
using System.Reactive;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingSessionViewModel : SessionViewModel
{
    public ModelCreatingViewModel ModelCreating { get; }
    // public ModelCreatingSettingsViewModel ModelCreatingSettings { get; }
    public ModelCreatingSessionViewModel(ModelCreatingSessionModelView modelCreatingSessionMv)
    {
        ModelCreating = new ModelCreatingViewModel(modelCreatingSessionMv.ModelCreating, modelCreatingSessionMv.Settings);
        // ModelCreatingSettings = new ModelCreatingSettingsViewModel(modelCreatingSessionMv.Settings);
        CurrentViewModel = ModelCreating;
        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    
    private ModelCreatingViewModelBase _currentViewModel;
    public ModelCreatingViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
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

    public ModelCreatingSessionViewModel(ModelCreatingSessionModelView modelCreatingSessionMv)
    {
        ModelCreating = new ModelCreatingViewModel(modelCreatingSessionMv.ModelCreating);
        ModelCreatingSettings = new ModelCreatingSettingsViewModel(modelCreatingSessionMv.Settings);
        CurrentViewModel = ModelCreatingSettings;
        
        OnClosingCommand = ReactiveCommand.Create(() =>
        {
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() => { });
    }
    
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}
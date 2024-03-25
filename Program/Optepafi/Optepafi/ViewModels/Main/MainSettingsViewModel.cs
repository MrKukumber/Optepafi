using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Optepafi.Models;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    private SearchingAlgorithm _selectedAlgorithm;
    private ElevSource _defaultElevSource;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, ElevSource> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, ElevSource>();
        ElevConfig = new ElevConfigViewModel();
        
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            DefaultElevSource = await ElevConfigInteraction.Handle(ElevConfig);
        });
    }
    
    public ObservableCollection<SearchingAlgorithm> Algorithms { get; }
    public SearchingAlgorithm? DefaultAlgorithm
    {
        get => _selectedAlgorithm;
        set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    }
    public ElevSource? DefaultElevSource
    {
        get => _defaultElevSource;
        set => this.RaiseAndSetIfChanged(ref _defaultElevSource, value);
    }
}
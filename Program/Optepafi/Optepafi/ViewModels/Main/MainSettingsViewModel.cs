using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Optepafi.Models;
using Optepafi.Models.AlgorithmMan;
using Optepafi.Models.ElevationDataMan;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    private ISearchAlgorithm _selectedAlgorithm;
    private IElevSource _defaultElevSource;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, IElevSource> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, IElevSource>();
        ElevConfig = new ElevConfigViewModel();
        
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            DefaultElevSource = await ElevConfigInteraction.Handle(ElevConfig);
        });
    }
    
    public ObservableCollection<ISearchAlgorithm> Algorithms { get; }
    public ISearchAlgorithm? DefaultAlgorithm
    {
        get => _selectedAlgorithm;
        set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    }
    public IElevSource? DefaultElevSource
    {
        get => _defaultElevSource;
        set => this.RaiseAndSetIfChanged(ref _defaultElevSource, value);
    }
}
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
    private ISearchingAlgorithm _selectedAlgorithm;
    private IElevDataSource _defaultElevDataSource;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, IElevDataSource> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, IElevDataSource>();
        ElevConfig = new ElevConfigViewModel();
        
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            DefaultElevSource = await ElevConfigInteraction.Handle(ElevConfig);
        });
    }
    
    public ObservableCollection<ISearchingAlgorithm> Algorithms { get; }
    public ISearchingAlgorithm? DefaultAlgorithm
    {
        get => _selectedAlgorithm;
        set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    }
    public IElevDataSource? DefaultElevSource
    {
        get => _defaultElevDataSource;
        set => this.RaiseAndSetIfChanged(ref _defaultElevDataSource, value);
    }
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia.Controls;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.ModelViews.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    // private ISearchingAlgorithm _selectedAlgorithm;
    private IElevDataSource? _currentElevDataSource;
    private CultureInfo _currentCulture;
    
    private readonly ParamsManagingModelView _paramsManager = ParamsManagingModelView.Instance;
    private MainSettingsParams _mainSettingsParams;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, IElevDataSource?> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {

        if ((_mainSettingsParams = _paramsManager.GetParams<MainSettingsParams>()!) is null)
        {
            _currentElevDataSource = null;
            CurrentCulture = CultureInfo.CurrentCulture;
            
            _mainSettingsParams = new MainSettingsParams { ElevDataSourceName = null, Culture = CurrentCulture.Name};
            _paramsManager.SetParams(_mainSettingsParams);
        }
        else
        {
            foreach (var elevDataSource in ElevDataModelView.Instance.ElevDataSources)
            {
                if (elevDataSource.GetType().Name == _mainSettingsParams.ElevDataSourceName)
                {
                    _currentElevDataSource = elevDataSource;
                }
            }
            CurrentCulture = CultureInfo.GetCultureInfo(_mainSettingsParams.Culture);
        }


        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture => _mainSettingsParams.Culture = currentCulture.Name);
        this.WhenAnyValue(x => x.CurrentElevDataSource)
            .Subscribe(currentElevDataSource => _mainSettingsParams.ElevDataSourceName = currentElevDataSource?.GetType().Name);

        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, IElevDataSource?>();
        ElevConfig = new ElevConfigViewModel(CurrentElevDataSource);
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentElevDataSource = await ElevConfigInteraction.Handle(ElevConfig);
        });
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
    }
    
    // public ObservableCollection<ISearchingAlgorithm> Algorithms { get; }
    // public ISearchingAlgorithm? DefaultAlgorithm
    // {
        // get => _selectedAlgorithm;
        // set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    // }

    public IElevDataSource? CurrentElevDataSource
    {
        get => _currentElevDataSource;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataSource, value);
    }

    public ObservableCollection<CultureInfo> Cultures { get; } = [CultureInfo.GetCultureInfo("en-GB"), CultureInfo.GetCultureInfo("sk-SK")];
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            Assets.Localization.Local.Culture = value;
            this.RaiseAndSetIfChanged(ref _currentCulture, value);
        }
        
    }
}
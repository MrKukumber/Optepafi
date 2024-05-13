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
    
    private ParamsManagingModelView _paramsManager = ParamsManagingModelView.Instance;
    private MainSettingsParams _mainSettingsParams;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, IElevDataSource> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {

        MainSettingsParams? mainSettingsParams;
        if ((mainSettingsParams = _paramsManager.GetParams<MainSettingsParams>()) is null)
        {
            _currentElevDataSource = null;
            _currentCulture = CultureInfo.CurrentCulture;
            
            _mainSettingsParams = new MainSettingsParams { ElevDataSourceName = null, Culture = _currentCulture.Name};
            _paramsManager.SetParams(_mainSettingsParams);
        }
        else
        {
            _mainSettingsParams = mainSettingsParams;
            
            foreach (var elevDataSource in ElevDataModelView.Instance.ElevDataSources)
            {
                if (elevDataSource.GetType().Name == mainSettingsParams.ElevDataSourceName)
                {
                    _currentElevDataSource = elevDataSource;
                }
            }
            _currentCulture = CultureInfo.GetCultureInfo(mainSettingsParams.Culture);
            Assets.Localization.Local.Culture = _currentCulture;
        }


        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture =>
            {
                Assets.Localization.Local.Culture = currentCulture;
                _mainSettingsParams.Culture = currentCulture.Name;
            });
        this.WhenAnyValue(x => x.CurrentElevDataSource)
            .Subscribe(currentElevDataSource => _mainSettingsParams.ElevDataSourceName = currentElevDataSource?.GetType().Name);

        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, IElevDataSource>();
        ElevConfig = new ElevConfigViewModel();
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
        set => this.RaiseAndSetIfChanged(ref _currentCulture, value);
    }

}
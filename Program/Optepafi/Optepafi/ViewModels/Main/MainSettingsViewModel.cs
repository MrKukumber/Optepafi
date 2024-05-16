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
using Optepafi.ViewModels.Main.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    // private ISearchingAlgorithm _selectedAlgorithm;
    private ElevDataTypeViewModel? _currentElevDataType;
    private CultureInfo _currentCulture;
    
    private readonly ParamsManagingModelView _paramsManager = ParamsManagingModelView.Instance;
    private MainSettingsParams _mainSettingsParams;
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, ElevDataTypeViewModel?> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
    public MainSettingsViewModel()
    {

        if ((_mainSettingsParams = _paramsManager.GetParams<MainSettingsParams>()!) is null)
        {
            _currentElevDataType = null;
            _currentCulture = CultureInfo.CurrentCulture;
            Assets.Localization.Local.Culture = _currentCulture;
            
            _mainSettingsParams = new MainSettingsParams { ElevDataTypeTypeName = null, Culture = _currentCulture.Name};
            _paramsManager.SetParams(_mainSettingsParams);
        }
        else
        {
            
            _currentElevDataType = _mainSettingsParams.ElevDataTypeTypeName is null ? null : ElevDataModelView.Instance.GetElevDataType(_mainSettingsParams.ElevDataTypeTypeName);
            _currentCulture = CultureInfo.GetCultureInfo(_mainSettingsParams.Culture);
            Assets.Localization.Local.Culture = _currentCulture;
        }


        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture => _mainSettingsParams.Culture = currentCulture.Name);
        this.WhenAnyValue(x => x.CurrentElevDataType)
            .Subscribe(currentElevDataSource => _mainSettingsParams.ElevDataTypeTypeName = currentElevDataSource?.GetType().Name);

        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, ElevDataTypeViewModel?>();
        ElevConfig = new ElevConfigViewModel(_currentElevDataType);
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentElevDataType = await ElevConfigInteraction.Handle(ElevConfig);
        });
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
    }
    
    // public ObservableCollection<ISearchingAlgorithm> Algorithms { get; }
    // public ISearchingAlgorithm? DefaultAlgorithm
    // {
        // get => _selectedAlgorithm;
        // set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    // }

    public ElevDataTypeViewModel? CurrentElevDataType
    {
        get => _currentElevDataType;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataType, value);
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
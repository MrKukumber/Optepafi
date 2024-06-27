using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using Optepafi.Models.ParamsMan;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel which is responsible for control over main settings of application.
/// Its tasks include:
/// - overseeing of main parameters setting by user.
/// - providing interaction with elevation data configuration
/// - providing collection of all cultures to which is application localized
///
/// For more information on ViewModels in general see <see cref="ViewModelBase"/>.
/// </summary>
public class MainSettingsViewModel : ViewModelBase
{
    /// <summary>
    /// Corresponding ModelView to this ViewModel used for providing data and services ver Model layer of application.k
    /// </summary>
    private readonly MainSettingsModelView _mainSettingsMv;
    
    /// <summary>
    /// Construction of main settings ViewModel instance.
    /// It initialize all reactive constructs.
    /// It also initialize main parameters according to saved parameters in previous run of application. It uses ModelViews services for this purpose.
    /// </summary>
    /// <param name="mainSettingsMv">Corresponding ModelView to this ViewModel.</param>
    public MainSettingsViewModel(MainSettingsModelView mainSettingsMv)
    {
        _mainSettingsMv = mainSettingsMv;

        _currentElevDataDistribution = mainSettingsMv.CurrentElevDataDistribution;
        _currentCulture = mainSettingsMv.CurrentCulture;
        Assets.Localization.Local.Culture = _currentCulture;
        
        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture => _mainSettingsMv.CurrentCulture = currentCulture);
        this.WhenAnyValue(x => x.CurrentElevDataDistribution)
            .Subscribe(currentElevDataDistribution => _mainSettingsMv.CurrentElevDataDistribution = currentElevDataDistribution); 
        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?>();
        var elevDataConfig = new ElevConfigViewModel(_currentElevDataDistribution);
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentElevDataDistribution = await ElevConfigInteraction.Handle(elevDataConfig);
        });
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        
        
    }
    
    /// <summary>
    /// Parameter that indicates currently chosen elevation data distribution that should be used in whole application for elevation data retrieval.
    /// It raises notification about change of its value.
    /// </summary>
    public ElevDataDistributionViewModel? CurrentElevDataDistribution
    {
        get => _currentElevDataDistribution;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataDistribution, value);
    }
    private ElevDataDistributionViewModel? _currentElevDataDistribution;

    /// <summary>
    /// Collection of all cultures to which is can be application localized.
    /// </summary>
    public ObservableCollection<CultureInfo> Cultures { get; } = [CultureInfo.GetCultureInfo("en-GB"), CultureInfo.GetCultureInfo("sk-SK")];
    /// <summary>
    /// Parameter which indicate currently used culture in the application.
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set => this.RaiseAndSetIfChanged(ref _currentCulture, value);
    }
    private CultureInfo _currentCulture;
    
    /// <summary>
    /// Reactive command which initiate change of currently used ViewModel in <c>MainWindowViewModel</c> to manin menu ViewModel.
    /// </summary>
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    /// <summary>
    /// Reactive command for showing elevation data configuration to user so he could manage elevation data download and removal.
    /// It is also used for setting of currently used elevation data distribution.
    /// Command calls for handling of elevation data configuration interaction. This interaction is handled by corresponding View.
    /// After interactions end its returned value is set to currently used data distribution parameter. 
    /// </summary>
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    /// <summary>
    /// Interaction to be handled when elevation data configuration mechanism should be provided to user.
    /// Corresponding View should implement handler for this interaction nad secure its correct execution.
    /// Argument of this interaction is elevation data configuration ViewModel which should be used for control over configuration process.
    /// The returning value indicates, which elevation data distribution was selected to be currently used default distribution in whole application.
    /// </summary>
    public Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?> ElevConfigInteraction { get; }
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel which is responsible for control over main settings of application.
/// 
/// Its tasks include:
/// 
/// - overseeing of main parameters setting by user.
/// - providing interaction with elevation data configuration
/// - providing collection of all cultures to which is application localized
/// - provides instance of <c>Provider</c> class which can be offered to sessions for safe accessing of main parameters
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
    /// 
    /// It initialize all reactive constructs.  
    /// It also initialize main parameters according to saved parameters in previous run of application. It uses ModelViews services for this purpose.  
    /// </summary>
    /// <param name="mainSettingsMv">Corresponding ModelView to this ViewModel.</param>
    public MainSettingsViewModel(MainSettingsModelView mainSettingsMv)
    {
        _mainSettingsMv = mainSettingsMv;

        CurrentElevDataDistribution = mainSettingsMv.CurrentElevDataDistribution;
        CurrentCulture = mainSettingsMv.CurrentCulture;
        Assets.Localization.MainWindowLocal.Culture = CurrentCulture;
        Assets.Localization.PathFindingLocal.Culture = CurrentCulture;
        
        ProviderOfSettings = new Provider(this);
        
        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture => _mainSettingsMv.CurrentCulture = currentCulture);
        this.WhenAnyValue(x => x.CurrentElevDataDistribution)
            .Subscribe(currentElevDataDistribution => _mainSettingsMv.CurrentElevDataDistribution = currentElevDataDistribution); 
        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?>();
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentElevDataDistribution = await ElevConfigInteraction.Handle(new ElevConfigViewModel(CurrentElevDataDistribution));
        });
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
    }
    
    /// <summary>
    /// Provider of settings that can be safely offered to outside word.
    /// </summary>
    public Provider ProviderOfSettings { get; }
    
    /// <summary>
    /// Parameter that indicates currently chosen elevation data distribution that should be used in whole application for elevation data retrieval.
    /// 
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
        set
        {
            Assets.Localization.MainWindowLocal.Culture = value;
            Assets.Localization.PathFindingLocal.Culture = value;
            this.RaiseAndSetIfChanged(ref _currentCulture, value);
        }
    }

    private CultureInfo _currentCulture;
    
    /// <summary>
    /// Reactive command which initiate change of currently used ViewModel in <c>MainWindowViewModel</c> to manin menu ViewModel.
    /// </summary>
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    /// <summary>
    /// Reactive command for showing elevation data configuration to user so he could manage elevation data download and removal.
    /// 
    /// It is also used for setting of currently used elevation data distribution.  
    /// Command calls for handling of elevation data configuration interaction. This interaction is handled by corresponding View. Always new instance of <c>ElevConfigViewModel</c> is passed to interaction.  
    /// After interactions end its returned value is set to currently used data distribution parameter.  
    /// </summary>
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    /// <summary>
    /// Interaction to be handled when elevation data configuration mechanism should be provided to user.
    /// 
    /// Corresponding View should implement handler for this interaction nad secure its correct execution.  
    /// Argument of this interaction is elevation data configuration ViewModel which should be used for control over configuration process.  
    /// The returning value indicates, which elevation data distribution was selected to be currently used default distribution in whole application.  
    /// </summary>
    public Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?> ElevConfigInteraction { get; }
    
    
    /// <summary>
    /// Provider of main settings parameters.
    /// 
    /// Its main task is to safely provide these parameters to outer world such as sessions.  
    /// Its instance is provided by <c>MainSettingsModelView</c> in <c>ProviderOfSettings</c> property.  
    /// </summary>
    public class Provider : ReactiveObject
    {
        public Provider(MainSettingsViewModel providedMainSettingsVm)
        {
            _providedMainSettingsVm = providedMainSettingsVm;
            _currentCutlure = providedMainSettingsVm.WhenAnyValue(x => x.CurrentCulture)
                .ToProperty(this, nameof(CurrentCulture));
        }
        
        /// <summary>
        /// Main settings whose parameters shall be provided.
        /// </summary>
        private MainSettingsViewModel _providedMainSettingsVm;

        /// <summary>
        /// Currently used culture.
        /// </summary>
        public CultureInfo CurrentCulture => _currentCutlure.Value;
        private ObservableAsPropertyHelper<CultureInfo> _currentCutlure;
        
        /// <summary>
        /// Currently chosen elevation data distribution.
        /// </summary>
        public ElevDataDistributionViewModel? CurrentElevDataDistribution => _providedMainSettingsVm.CurrentElevDataDistribution;
    }
}
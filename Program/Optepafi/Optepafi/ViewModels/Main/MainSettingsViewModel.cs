using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using DynamicData;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Configuration;
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
    /// It initializes all reactive constructs.  
    /// It also initializes main parameters according to saved parameters in previous run of application. It uses ModelViews services for this purpose.  
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

        _selectedConfigurationTitle = this.WhenAnyValue(x => x.SelectedConfigurationNode,
                selectedNode => selectedNode is not InnerNode ? selectedNode?.Title : null)
            .ToProperty(this, nameof(SelectedConfigurationTitle));

        ConfigurationNodes = new List<Node>
        {
            new InnerNode("Searching algorithms", mainSettingsMv.SearchingAlgorithmsConfigurations.Select(kv => new SearchingAlgorithmConfigNode(kv.Key))),
            new InnerNode("Map representations", mainSettingsMv.MapRepresentationsConfigurations.Select(kv => new MapRepreConfigNode(kv.Key))),
            new InnerNode("User models", mainSettingsMv.UserModelsConfigurations.Select(kv => new UserModelConfigNode(kv.Key)))
        };
        SelectedConfigurationNode = null;
        
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
    
    //TODO:comment
    public IEnumerable<Node> ConfigurationNodes { get; }

    public Node? SelectedConfigurationNode
    {
        get => _selectedConfigurationNode; 
        set => this.RaiseAndSetIfChanged(ref _selectedConfigurationNode, value);
    }
    private Node? _selectedConfigurationNode;

    private ObservableAsPropertyHelper<string?> _selectedConfigurationTitle;
    public string? SelectedConfigurationTitle => _selectedConfigurationTitle.Value;
    
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

        public Dictionary<SearchingAlgorithmViewModel, ConfigurationViewModel> SearchingAlgorithmsConfigurations =>
            _providedMainSettingsVm._mainSettingsMv.SearchingAlgorithmsConfigurations;
        public Dictionary<MapRepreRepresentativeViewModel, ConfigurationViewModel> MapRepreConfigurations =>
            _providedMainSettingsVm._mainSettingsMv.MapRepresentationsConfigurations;
        public Dictionary<UserModelTypeViewModel, ConfigurationViewModel> UserModelsConfigurations =>
            _providedMainSettingsVm._mainSettingsMv.UserModelsConfigurations;

    } 
    public abstract class Node
    {
        public virtual IEnumerable<Node>? SubNodes { get; } = null;
        public virtual ConfigurationViewModel? Configuration { get; } = null;
        public abstract string Title { get; }
    }

    public class InnerNode : Node
    {
        public override IEnumerable<Node>? SubNodes { get; }
        public override string Title { get; }

        public InnerNode(string title, IEnumerable<Node>? subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
    }

    public class MapRepreConfigNode : Node
    {
        public override string Title { get; }
        public MapRepreRepresentativeViewModel MapRepreRepresentative { get; }

        public MapRepreConfigNode(MapRepreRepresentativeViewModel mapRepreRepresentative)
        {
            MapRepreRepresentative = mapRepreRepresentative;
            Title = MapRepreRepresentative.MapRepreName;
        }
    }

    public class SearchingAlgorithmConfigNode : Node
    {
        public override string Title { get; }
        public SearchingAlgorithmViewModel SearchingAlgorithm { get; }

        public SearchingAlgorithmConfigNode(SearchingAlgorithmViewModel searchingAlgorithm)
        {
            SearchingAlgorithm = searchingAlgorithm;
            Title = SearchingAlgorithm.Name;
        }
    }

    public class UserModelConfigNode : Node
    {
        public override string Title { get; }
        public UserModelTypeViewModel UserModelType { get; }

        public UserModelConfigNode(UserModelTypeViewModel userModelType)
        {
            UserModelType = userModelType;
            Title = UserModelType.UserModelTypeName;
        }
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Configuration;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ModelViews.Main;

/// <summary>
/// ModelView which is responsible for logic behind management of main settings.
/// 
/// Its work load includes:
/// - managing parameters which are used by whole application
/// - setting of current culture of application and thus providing its localization
/// - work with <c>ParamsManager</c> for achieving persistence of applications parameters
/// - work with <c>ElevDataManager</c> for correct identification of elevation data distributions
/// 
/// For more information on ModelViews see <see cref="ModelViewBase"/>.  
/// </summary>
public class MainSettingsModelView : ModelViewBase
{
    /// <summary>
    /// Instance of <c>MainSettingsParams</c> which is used for changing and following saving of main parameters. 
    /// </summary>
    private MainSettingsParams _mainSettingsParams;

    private ConfigurationsParams _configurationsParams;
    
    /// <summary>
    /// When new instance is created, main parameters are initialized from saved parameters.
    /// </summary>
    public MainSettingsModelView()
    {
        _mainSettingsParams = ParamsManager.Instance.GetParams<MainSettingsParams>() ?? new MainSettingsParams
            { ElevDataTypeViewModelTypeName = null, CultureName = CultureInfo.CurrentCulture.Name};
        ParamsManager.Instance.SetParams(_mainSettingsParams);
        
        _currentCulture = CultureInfo.GetCultureInfo(_mainSettingsParams.CultureName);
        _currentElevDataDistribution = GetElevDataDistributionByTypeName(_mainSettingsParams.ElevDataTypeViewModelTypeName);

        _configurationsParams = ParamsManager.Instance.GetParams<ConfigurationsParams>() ?? new ConfigurationsParams();
        SearchingAlgorithmsConfigurations = SearchingAlgorithmManager.Instance.SearchingAlgorithms
            .Select(searchingAlgorithm =>
            {
                if (_configurationsParams.Configurations.TryGetValue(searchingAlgorithm.DefaultConfigurationDeepCopy.GetType(), out var configuration))
                    return (searchingAlgorithm, configuration);
                var defaultConfigurationCopy = searchingAlgorithm.DefaultConfigurationDeepCopy;
                _configurationsParams.Configurations[searchingAlgorithm.DefaultConfigurationDeepCopy.GetType()] = defaultConfigurationCopy;
                return (searchingAlgorithm, defaultConfigurationCopy);
            }).ToDictionary(x => new SearchingAlgorithmViewModel(x.Item1), x => new ConfigurationViewModel(x.Item2));
        MapRepresentationsConfigurations = MapRepreManager.Instance.MapRepreReps
            .Select(mapRepreRepresentative =>
            {
                if (_configurationsParams.Configurations.TryGetValue(mapRepreRepresentative.DefaultConfigurationDeepCopy.GetType(), out var configuration))
                    return (mapRepreRepresentative, configuration);
                var defaultConfigurationCopy = mapRepreRepresentative.DefaultConfigurationDeepCopy;
                _configurationsParams.Configurations[mapRepreRepresentative.DefaultConfigurationDeepCopy.GetType()] = defaultConfigurationCopy;
                return (mapRepreRepresentative, defaultConfigurationCopy);
            }).ToDictionary(x => new MapRepreRepresentativeViewModel(x.Item1) , x => new ConfigurationViewModel(x.Item2) );
        UserModelsConfigurations = UserModelManager.Instance.UserModelTypes
            .Select(userModelType =>
            {
                if (_configurationsParams.Configurations.TryGetValue(userModelType.DefaultConfigurationDeepCopy.GetType(), out var configuration)) 
                    return (userModelType, configuration);
                var defaultConfigurationCopy = userModelType.DefaultConfigurationDeepCopy;
                _configurationsParams.Configurations[userModelType.DefaultConfigurationDeepCopy.GetType()] = defaultConfigurationCopy;
                return (userModelType, defaultConfigurationCopy);

            }).ToDictionary(x => new UserModelTypeViewModel(x.Item1) , x => new ConfigurationViewModel(x.Item2));
    }
    /// <summary>
    /// Method for identifying elevation data distribution whose type corresponds to provided type name.
    /// It runs through all elevation data sources provided by <c>ElevDataManager</c> and through all of their distributions and looks for matching one.
    /// </summary>
    /// <param name="typeName">Name of distribution type which is looked for.</param>
    /// <returns>Matching elevation data distribution if found. Null otherwise.</returns>
    private IElevDataDistribution? GetElevDataDistributionByTypeName(string? typeName)
    {
        if (typeName is null) return null;
        foreach (var elevDataSource in ElevDataManager.Instance.ElevDataSources)
        {
            foreach (var elevDataDistrViewModel in elevDataSource.ElevDataDistributions)
            {
                if (elevDataDistrViewModel.GetType().Name == typeName)
                {
                    return elevDataDistrViewModel;
                }
            }
        }
        return null;
    }
    
    
    
    /// <summary>
    /// Backing field of <c>CurrentCulture</c> property.
    /// </summary>
    private CultureInfo _currentCulture;
    /// <summary>
    /// Represents current culture of application. When new value is set, it is also saved to <c>_mainSettingsParams</c> variable.
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            _currentCulture = value;
            _mainSettingsParams.CultureName = value.Name;
        }
    }

    /// <summary>
    /// Backing field of <c>CurrentElevDataDistribution</c> property.
    /// </summary>
    private IElevDataDistribution? _currentElevDataDistribution;
    /// <summary>
    /// Represents currently selected elevation data distributions ViewModel which is used in application.
    /// When new value is set, it is also saved to <c>_mainSettingsParams</c> variable.
    /// </summary>
    public ElevDataDistributionViewModel? CurrentElevDataDistribution
    {
        get
        {
            return _currentElevDataDistribution switch
            {
                ICredentialsNotRequiringElevDataDistribution credNotReqElevDataDist => new CredentialsNotRequiringElevDataDistributionViewModel(credNotReqElevDataDist),
                ICredentialsRequiringElevDataDistribution credReqElevDataDist => new CredentialsRequiringElevDataDistributionViewModel(credReqElevDataDist),
                _ => null
            };
        }
        set
        {
            _currentElevDataDistribution = value?.ElevDataDistribution;
            _mainSettingsParams.ElevDataTypeViewModelTypeName = value?.ElevDataDistribution.GetType().Name;
        }
    }

    //TODO: comment
    public Dictionary<SearchingAlgorithmViewModel, ConfigurationViewModel> SearchingAlgorithmsConfigurations { get; }
    public Dictionary<MapRepreRepresentativeViewModel, ConfigurationViewModel> MapRepresentationsConfigurations { get; }
    public Dictionary<UserModelTypeViewModel, ConfigurationViewModel> UserModelsConfigurations { get; }

}
using System.Globalization;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.ViewModels.Data.Representatives;

namespace Optepafi.ModelViews.Main;

/// <summary>
/// ModelView which is responsible for logic behind management of main settings.
/// Its work load includes:
/// - managing parameters which are used by whole application
/// - setting of current culture of application and thus providing its localization
/// - work with <c>ParamsManager</c> for achieving persistence of applications parameters
/// - work with <c>ElevDataManager</c> for correct identification of elevation data distributions
/// - provides instance of <c>Provider</c> class which can be offered to sessions for safe accessing of main parameters
/// For more information on ModelViews see <see cref="ModelViewBase"/>.
/// </summary>
public class MainSettingsModelView : ModelViewBase
{
    /// <summary>
    /// Instance of <c>MainSettingsParams</c> which is used for changing and following saving of main parameters. 
    /// </summary>
    private MainSettingsParams _mainSettingsParams;
    
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
        ProviderOfSettings = new Provider(this);
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
    /// Provider of settings that can be safely offered to outside word.
    /// </summary>
    public Provider ProviderOfSettings { get; }
    
    
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

    /// <summary>
    /// Provider of main settings parameters.
    /// Its main task is to safely provide these parameters to outer world such as sessions.
    /// Its instance is provided by <c>MainSettingsModelView</c> in <c>ProviderOfSettings</c> property.
    /// </summary>
    public class Provider(MainSettingsModelView providedMainSettingsMv)
    {
        /// <summary>
        /// Main settings whose parameters shall be provided.
        /// </summary>
        private MainSettingsModelView _providedMainSettingsMv = providedMainSettingsMv;
        /// <summary>
        /// Currently used culture.
        /// </summary>
        public CultureInfo CurrentCulture => _providedMainSettingsMv.CurrentCulture;
        /// <summary>
        /// Currently chosen elevation data distribution.
        /// </summary>
        public ElevDataDistributionViewModel? CurrentElevDataDistribution => _providedMainSettingsMv.CurrentElevDataDistribution;
    }
}
using System.Globalization;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.Main;

public class MainSettingsModelView
{
    private MainSettingsParams _mainSettingsParams;
    public MainSettingsModelView()
    {
        _mainSettingsParams = ParamsManager.Instance.GetParams<MainSettingsParams>() ?? new MainSettingsParams
            { ElevDataTypeViewModelTypeName = null, CultureName = CultureInfo.CurrentCulture.Name};
        ParamsManager.Instance.SetParams(_mainSettingsParams);
        
        _currentCulture = CultureInfo.GetCultureInfo(_mainSettingsParams.CultureName);
        _currentElevDataType = GetElevDataTypeByTypeName(_mainSettingsParams.ElevDataTypeViewModelTypeName);
        ProviderOfSettings = new Provider(this);
    }
    private IElevDataDistribution? GetElevDataTypeByTypeName(string? typeName)
    {
        if (typeName is null) return null;
        foreach (var elevDataSource in ElevDataManager.Instance.ElevDataSources)
        {
            foreach (var elevDataTypeViewModel in elevDataSource.ElevDataDistributions)
            {
                if (elevDataTypeViewModel.GetType().Name == typeName)
                {
                    return elevDataTypeViewModel;
                }
            }
        }
        return null;
    }
    
    public Provider ProviderOfSettings { get; }
    
    private CultureInfo _currentCulture;
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            _currentCulture = value;
            _mainSettingsParams.CultureName = value.Name;
        }
    }

    private IElevDataDistribution? _currentElevDataType;
    public ElevDataDistributionViewModel? CurrentElevDataDistribution
    {
        get => _currentElevDataType is null ? null : new ElevDataDistributionViewModel(_currentElevDataType);
        set
        {
            _currentElevDataType = value?.ElevDataDistribution;
            _mainSettingsParams.ElevDataTypeViewModelTypeName = value?.ElevDataDistribution.GetType().Name;
        }
    }

    public class Provider
    {
        private MainSettingsModelView _providedmainSettingsMv;
        public Provider(MainSettingsModelView providedMainSettingsMv)
        {
            _providedmainSettingsMv = providedMainSettingsMv;
        }

        public CultureInfo CurrentCutlure => _providedmainSettingsMv.CurrentCulture;
        public ElevDataDistributionViewModel? CurrentElevDataDistribution => _providedmainSettingsMv.CurrentElevDataDistribution;
    }
}
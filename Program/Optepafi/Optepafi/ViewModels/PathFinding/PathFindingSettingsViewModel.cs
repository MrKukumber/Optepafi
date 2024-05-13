using System.Dynamic;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSettingsViewModel : ViewModelBase
{
    public PFSettingsModelView SettingsMv { get;}
    public ParamsManagingModelView ParamsManagingMv { get; }
    public PathFindingSettingsViewModel(PFSettingsModelView settingsesMv)
    {
        SettingsMv = settingsesMv;
        ParamsManagingMv = ParamsManagingModelView.Instance;
    }
    
}
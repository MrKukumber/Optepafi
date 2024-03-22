using System.Dynamic;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSettingsViewModel : ViewModelBase
{
    public PFSettingsModelView SettingsMv { get;}
    public MainParamsManagingModelView MainParamsManagingMv { get; }
    public PathFindingSettingsViewModel(PFSettingsModelView settingsesMv, MainParamsManagingModelView mainParamsManagingMv)
    {
        SettingsMv = settingsesMv;
        MainParamsManagingMv = mainParamsManagingMv;
    }
    
}
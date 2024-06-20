using Optepafi.Models.ParamsMan;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingSettingsViewModel : ModelCreatingViewModelBase
{
    public MCSettingsModelView SettingsMv { get; }
    public ParamsManager ParamsManaging { get; }

    public ModelCreatingSettingsViewModel(MCSettingsModelView settingsMv)
    {
        SettingsMv = settingsMv;
        ParamsManaging = ParamsManager.Instance;
    }
}
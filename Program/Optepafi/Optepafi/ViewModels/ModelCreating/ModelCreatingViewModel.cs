using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingViewModel : ModelCreatingViewModelBase
{
    public MCModelCreatingModelView ModelCreatingMv {get;}
    public MCSettingsModelView SettingsMv { get; }
    public ModelCreatingViewModel(MCModelCreatingModelView modelCreatingMv, MCSettingsModelView settingsMv)
    {
        ModelCreatingMv = modelCreatingMv;
        SettingsMv = settingsMv;
    }
}
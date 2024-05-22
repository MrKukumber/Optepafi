using Optepafi.Models.ParamsMan;

namespace Optepafi.ModelViews.Main;

public class MainWindowModelView
{
    public MainSettingsModelView MainSettings { get; }

    public MainWindowModelView()
    {
        MainSettings = new MainSettingsModelView();
    }

    public void SaveParams()
    {
        ParamsManager.Instance.SaveAllParams();
    }
}
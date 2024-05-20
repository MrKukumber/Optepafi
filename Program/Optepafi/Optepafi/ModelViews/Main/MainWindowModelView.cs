namespace Optepafi.ModelViews.Main;

public class MainWindowModelView
{
    public MainSettingsModelView MainSettings { get; }

    public MainWindowModelView()
    {
        MainSettings = new MainSettingsModelView();
    }
}
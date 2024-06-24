using Optepafi.Models.ParamsMan;

namespace Optepafi.ModelViews.Main;

/// <summary>
/// ModelView for main window. It is responsible for logic behind main window.
/// Its only duty for now is to let <c>ParamsManager</c> correctly save parameters when requested.
/// </summary>
public class MainWindowModelView
{
    public MainSettingsModelView MainSettings { get; } = new MainSettingsModelView();
    /// <summary>
    /// Method which lets <c>ParamsManager</c> correctly save parameters.
    /// </summary>
    public void SaveParams()
    {
        ParamsManager.Instance.SaveAllParams();
    }
}
using Optepafi.Models.ElevationDataMan;

namespace MapRepreViewer.ModelViews;

public class MainWindowModelView : ModelViewBase
{
    public MapRepreViewingModelView MapRepreViewing { get; } = new MapRepreViewingModelView();

    public void Initialize()
    {
        ElevDataManager.Instance.Initialize();
    }
}
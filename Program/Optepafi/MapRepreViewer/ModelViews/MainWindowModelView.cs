namespace MapRepreViewer.ModelViews;

public class MainWindowModelView : ModelViewBase
{
    public MapRepreViewingModelView MapRepreViewing { get; } = new MapRepreViewingModelView();
}
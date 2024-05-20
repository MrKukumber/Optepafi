namespace Optepafi.ModelViews.PathFinding;

public class PFMapRepreCreatingModelView : ModelViewBase
{
    protected PFMapRepreCreatingModelView(){}
}
public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFMapRepreCreatingIntraModelView : PFMapRepreCreatingModelView
    {
        private PFSettingsIntraModelView Settings { get; }

        public PFMapRepreCreatingIntraModelView(PFSettingsIntraModelView settings)
        {
            Settings = settings;
        }
    }
}

namespace Optepafi.ModelViews.ModelCreating;

public class PFGraphCreatingModelView : ModelViewBase
{
    protected PFGraphCreatingModelView(){}
}
public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFGraphCreatingIntraModelView : PFGraphCreatingModelView
    {
        private PFSettingsIntraModelView Settings { get; }

        public PFGraphCreatingIntraModelView(PFSettingsIntraModelView settings)
        {
            Settings = settings;
        }
    }
}

namespace Optepafi.ModelViews.PathFinding;

public class PFRelevanceFeedbackModelView : ModelViewBase
{
    protected PFRelevanceFeedbackModelView(){}
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFRelevanceFeedbackIntraModelView : PFRelevanceFeedbackModelView
    {
        private PFSettingsIntraModelView Settings { get; }
        private PFMapRepreCreatingIntraModelView MapRepreCreating { get; }

        public PFRelevanceFeedbackIntraModelView(PFSettingsIntraModelView settings, PFMapRepreCreatingIntraModelView mapRepreCreating)
        {
            Settings = settings;
            MapRepreCreating = mapRepreCreating;
        }
    }
}
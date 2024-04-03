namespace Optepafi.ModelViews.ModelCreating;

public class PFRelevanceFeedbackModelView : ModelViewBase
{
    protected PFRelevanceFeedbackModelView(){}
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFRelevanceFeedbackIntraModelView : PFRelevanceFeedbackModelView
    {
        private PFSettingsIntraModelView Settings { get; }
        private PFGraphCreatingIntraModelView GraphCreating { get; }

        public PFRelevanceFeedbackIntraModelView(PFSettingsIntraModelView settings, PFGraphCreatingIntraModelView graphCreating)
        {
            Settings = settings;
            GraphCreating = graphCreating;
        }
    }
}
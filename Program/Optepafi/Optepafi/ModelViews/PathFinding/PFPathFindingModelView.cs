namespace Optepafi.ModelViews.ModelCreating;

public class PFPathFindingModelView : ModelViewBase
{
    protected PFPathFindingModelView(){}
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFPathFindingIntraModelView : PFPathFindingModelView
    {
        private PFRelevanceFeedbackIntraModelView RelevanceFeedback { get; }

        public PFPathFindingIntraModelView(PFRelevanceFeedbackIntraModelView relevanceFeedback)
        {
            RelevanceFeedback = relevanceFeedback;
        }
    }
}
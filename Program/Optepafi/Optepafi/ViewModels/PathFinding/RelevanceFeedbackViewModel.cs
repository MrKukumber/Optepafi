using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;

namespace Optepafi.ViewModels.PathFinding;

public class RelevanceFeedbackViewModel : PathFindingViewModelBase
{
    public PFRelevanceFeedbackModelView RelevanceFeedbackMV { get; }
    public RelevanceFeedbackViewModel(PFRelevanceFeedbackModelView relevanceFeedbackMv)
    {
        RelevanceFeedbackMV = relevanceFeedbackMv;
    }
}
using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.PathFinding;

public class RelevanceFeedbackViewModel : ViewModelBase
{
    public PFRelevanceFeedbackModelView RelevanceFeedbackMV { get; }
    public RelevanceFeedbackViewModel(PFRelevanceFeedbackModelView relevanceFeedbackMv)
    {
        RelevanceFeedbackMV = relevanceFeedbackMv;
    }
}
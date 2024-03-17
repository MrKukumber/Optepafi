namespace Optepafi.ViewModels.PathFinding;

public class RelevanceFeedbackViewModel : ViewModelBase
{
    
    private PathFindingWindowViewModel ParentPahtFindingWindow { get; }
    public RelevanceFeedbackViewModel(PathFindingWindowViewModel parentPathFindingWindow)
    {
        ParentPahtFindingWindow = parentPathFindingWindow;
    }
}
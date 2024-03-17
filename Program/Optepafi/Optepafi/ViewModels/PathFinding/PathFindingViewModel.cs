namespace Optepafi.ViewModels.PathFinding;

public class PathFindingViewModel : ViewModelBase
{
    private PathFindingWindowViewModel ParentPahtFindingWindow { get; }
    public PathFindingViewModel(PathFindingWindowViewModel parentPathFindingWindow)
    {
        ParentPahtFindingWindow = parentPathFindingWindow;
    }
}
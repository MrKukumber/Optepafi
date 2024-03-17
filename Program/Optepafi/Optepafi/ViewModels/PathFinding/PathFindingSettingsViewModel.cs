namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSettingsViewModel : ViewModelBase
{
    private PathFindingWindowViewModel ParentPahtFindingWindow { get; }
    public PathFindingSettingsViewModel(PathFindingWindowViewModel parentPathFindingWindow)
    {
        ParentPahtFindingWindow = parentPathFindingWindow;
    }
    
}
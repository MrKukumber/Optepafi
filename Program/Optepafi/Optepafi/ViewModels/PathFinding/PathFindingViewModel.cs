using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingViewModel : ViewModelBase
{
    public PFPathFindingModelView PathFindingMv { get; }
    public PathFindingViewModel(PFPathFindingModelView pathFindingMv)
    {
        PathFindingMv = pathFindingMv;
    }
}
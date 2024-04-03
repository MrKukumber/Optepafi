namespace Optepafi.ModelViews.ModelCreating;

public class MCModelCreatingModelView : ModelViewBase
{
    protected MCModelCreatingModelView(){}
}

public partial class ModelCreatingSessionModelView : SessionModelView
{
    private class MCModelCreatingIntramodelView : MCModelCreatingModelView
    {
        private MCSettingsIntraModelView Settings { get; }
        private MCGraphCreatingIntraModelView GraphCreating { get; }
        private MCPathFindingIntraModelView PathFinding { get; }

        public MCModelCreatingIntramodelView(MCSettingsIntraModelView settings,
            MCGraphCreatingIntraModelView graphCreating, MCPathFindingIntraModelView pathFinding)
        {
            Settings = settings;
            GraphCreating = graphCreating;
            PathFinding = pathFinding;
        }
    }
}
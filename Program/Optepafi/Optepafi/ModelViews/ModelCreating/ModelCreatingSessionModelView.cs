namespace Optepafi.ModelViews.ModelCreating;

public class ModelCreatingSessionModelView : SessionModelView
{
    public MCSettingsModelView Settings { get; }
    public MCGraphCreatingModelView GraphCreating { get; }
    public MCModelCreatingModelView ModelCreating { get; }
    public MCPathFindingModelView PathFinding { get; }
    
    public ModelCreatingSessionModelView(/*TODO: vyzadovat model, na ktory sa napoja ModelView-vy, pokial nenajdem elgantnejsi sposob*/)
    {
        var settingsIntra = new MCSettingsIntraModelView();
        var graphCreatingIntra = new MCGraphCreatingIntraModelView(settingsIntra);
        var pathFindingIntra = new MCPathFindingIntraModelView();
        var modelCratingIntra = new MCModelCreatingIntramodelView(settingsIntra, graphCreatingIntra, pathFindingIntra);

        Settings = settingsIntra;
        GraphCreating = graphCreatingIntra;
        ModelCreating = modelCratingIntra;
        PathFinding = pathFindingIntra;
    }

    private class MCSettingsIntraModelView : MCSettingsModelView
    {
            
    }
    
    private class MCGraphCreatingIntraModelView : MCGraphCreatingModelView
    {
        private MCSettingsIntraModelView Settings { get; }

        public MCGraphCreatingIntraModelView(MCSettingsIntraModelView settings)
        {
            Settings = settings;
        }
    }
    
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

    private class MCPathFindingIntraModelView : MCPathFindingModelView
    {

    }

}
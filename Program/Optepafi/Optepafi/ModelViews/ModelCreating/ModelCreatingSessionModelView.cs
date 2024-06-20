namespace Optepafi.ModelViews.ModelCreating;

public partial class ModelCreatingSessionModelView : SessionModelView
{
    public MCSettingsModelView Settings { get; }
    // public MCGraphCreatingModelView GraphCreating { get; }
    public MCModelCreatingModelView ModelCreating { get; }
    // public MCPathFindingModelView PathFinding { get; }
    
    public ModelCreatingSessionModelView(/*TODO: vyzadovat model, na ktory sa napoja ModelView-vy, pokial nenajdem elgantnejsi sposob*/)
    {
        var settingsIntra = new MCSettingsIntraModelView();
        // var graphCreatingIntra = new MCGraphCreatingIntraModelView(settingsIntra);
        // var pathFindingIntra = new MCPathFindingIntraModelView();
        var modelCratingIntra = new MCModelCreatingIntramodelView(settingsIntra/*, graphCreatingIntra, pathFindingIntra*/);

        Settings = settingsIntra;
        // GraphCreating = graphCreatingIntra;
        ModelCreating = modelCratingIntra;
        // PathFinding = pathFindingIntra;
    }

}
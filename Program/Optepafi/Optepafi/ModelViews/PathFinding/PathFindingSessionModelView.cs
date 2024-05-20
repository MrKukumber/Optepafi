namespace Optepafi.ModelViews.PathFinding;

public partial class PathFindingSessionModelView : SessionModelView
{
    public PFSettingsModelView Settings { get; }
    public PFMapRepreCreatingModelView MapRepreCreating { get; }
    public PFRelevanceFeedbackModelView RelevanceFeedback { get; }
    public PFPathFindingModelView PathFinding { get; }
    
    public PathFindingSessionModelView()
    {
        /*Model budu singletony, ktore proste na vyziadanie budu dorucovat sluzby*/
        var settingsIntra = new PFSettingsIntraModelView();
        var graphCreatingIntra = new PFMapRepreCreatingIntraModelView(settingsIntra);
        var relevanceFeedbackIntra = new PFRelevanceFeedbackIntraModelView(settingsIntra, graphCreatingIntra);
        var pathFindingIntra = new PFPathFindingIntraModelView(relevanceFeedbackIntra);

        Settings = settingsIntra;
        MapRepreCreating = graphCreatingIntra;
        RelevanceFeedback = relevanceFeedbackIntra;
        PathFinding = pathFindingIntra;

    }

    
    
}
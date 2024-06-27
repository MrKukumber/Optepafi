namespace Optepafi.ModelViews.PathFinding;

/// <summary>
/// Path finding sessions ModelView. It contains all ModelViews which contribute to effort of delivering of path finding mechanism to the user.
/// For more information about session ModelViews see <see cref="SessionModelView"/>.
/// </summary>
public partial class PathFindingSessionModelView : SessionModelView
{
    public PFSettingsModelView Settings { get; }
    public PFMapRepreCreatingModelView MapRepreCreating { get; }
    public PFRelevanceFeedbackModelView RelevanceFeedback { get; }
    public PFPathFindingModelView PathFinding { get; }
    
    public PathFindingSessionModelView()
    {
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
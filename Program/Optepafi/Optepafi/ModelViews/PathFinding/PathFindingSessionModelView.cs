using System.IO;

namespace Optepafi.ModelViews.ModelCreating;

public partial class PathFindingSessionModelView : SessionModelView
{
    public PFSettingsModelView Settings { get; }
    public PFGraphCreatingModelView GraphCreating { get; }
    public PFRelevanceFeedbackModelView RelevanceFeedback { get; }
    public PFPathFindingModelView PathFinding { get; }
    
    public PathFindingSessionModelView()
    {
        /*Model budu staticke triedy, ktore proste na vyziadanie budu dorucovat sluzby*/
        var settingsIntra = new PFSettingsIntraModelView();
        var graphCreatingIntra = new PFGraphCreatingIntraModelView(settingsIntra);
        var relevanceFeedbackIntra = new PFRelevanceFeedbackIntraModelView(settingsIntra, graphCreatingIntra);
        var pathFindingIntra = new PFPathFindingIntraModelView(relevanceFeedbackIntra);

        Settings = settingsIntra;
        GraphCreating = graphCreatingIntra;
        RelevanceFeedback = relevanceFeedbackIntra;
        PathFinding = pathFindingIntra;

    }

    
    
}
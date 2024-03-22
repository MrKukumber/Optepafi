namespace Optepafi.ModelViews.ModelCreating;

public class PathFindingSessionModelView : SessionModelView
{
    public PFSettingsModelView Settings { get; }
    public PFGraphCreatingModelView GraphCreating { get; }
    public PFRelevanceFeedbackModelView RelevanceFeedback { get; }
    public PFPathFindingModelView PathFinding { get; }
    
    public PathFindingSessionModelView(/*TODO: vyzadovat model, na ktory sa napoja jednotlive ModelView-vy, ak teda nenajdem elegantnejsi sposob*/)
    {
        
    }
    
}
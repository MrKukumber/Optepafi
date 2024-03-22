namespace Optepafi.ModelViews.ModelCreating;

public class ModelCreatingSessionModelView : SessionModelView
{
    public MCSettingsModelView Settings { get; }
    public MCGraphCreatingModelView GraphCreating { get; }
    public MCModelCreatingModelView ModelCreating { get; }
    public MCPathFindingModelView PathFindingModelView { get; }
    
    public ModelCreatingSessionModelView(/*TODO: vyzadovat model, na ktory sa napoja ModelView-vy, pokial nenajdem elgantnejsi sposob*/)
    {
        
    }
}
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ModelViews.Main;

//TODO: comment
public class InitializationModelView : ModelViewBase
{
    public void InitializeResources()
    {
        ElevDataManager.Instance.Initialize();    
    }
}
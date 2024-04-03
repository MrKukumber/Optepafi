namespace Optepafi.ModelViews.ModelCreating;

public class MCGraphCreatingModelView : ModelViewBase
{
    protected MCGraphCreatingModelView(){}
}

public partial class ModelCreatingSessionModelView
{
    private class MCGraphCreatingIntraModelView : MCGraphCreatingModelView
    {
        private MCSettingsIntraModelView Settings { get; }

        public MCGraphCreatingIntraModelView(MCSettingsIntraModelView settings)
        {
            Settings = settings;
        }
    }
}
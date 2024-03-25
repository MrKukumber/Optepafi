namespace Optepafi.ModelViews.Main;

public sealed class ElevDataModelView : ModelViewBase
{
    
    private static ElevDataModelView _instance = new();
    private ElevDataModelView(){}
    public static ElevDataModelView Instance
    {
        get => _instance;
    }
}
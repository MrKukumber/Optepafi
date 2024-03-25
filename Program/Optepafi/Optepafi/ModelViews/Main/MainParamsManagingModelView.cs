namespace Optepafi.ModelViews.Main;

public sealed class MainParamsManagingModelView : ModelViewBase
{
    private static MainParamsManagingModelView _instance = new();
    private MainParamsManagingModelView(){}
    public static MainParamsManagingModelView Instance
    {
        get => _instance;
    }
}
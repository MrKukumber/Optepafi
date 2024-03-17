namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingViewModel : ViewModelBase
{
    private ModelCreatingWindowViewModel ParentModelCreatingWindow { get; }
    public ModelCreatingViewModel(ModelCreatingWindowViewModel parentModelCreatingWindow)
    {
        ParentModelCreatingWindow = parentModelCreatingWindow;
    }
}
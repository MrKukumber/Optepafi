using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingViewModel : ViewModelBase
{
    public MCModelCreatingModelView ModelCreatingMv {get;}
    public ModelCreatingViewModel(MCModelCreatingModelView modelCreatingMv)
    {
        ModelCreatingMv = modelCreatingMv;
    }
}
using ReactiveUI;

namespace Optepafi.ViewModels;

public class SessionViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
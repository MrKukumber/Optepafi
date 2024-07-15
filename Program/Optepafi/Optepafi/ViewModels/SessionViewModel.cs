using ReactiveUI;

namespace Optepafi.ViewModels;

/// <summary>
/// Base class for every session ViewModel.
///
/// Session ViewModels contain instance for every associated ViewModel. It represents the top layer of application logic.  
/// Sessions are huge abstract units that represents whole parts of application containing Views, ViewModels and ModelViews. These constructs work together to deliver applications mechanism to user.  
/// Session ViewModel main task is creating instances of associated ViewModels, passing appropriate ModelViews to them and managing of currently used ViewModel.  
/// Every session ViewModel is associated with some main session window which it uses for displaying of currently used ViewModels associated View.  
/// It should take care of any command/event which corresponding window emits, such as closing and closed commands. They should inform containing ViewModels about these commands/events and let them react to them.  
/// For more information on view models in general see <see cref="ViewModelBase"/>.  
/// </summary>
public class SessionViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
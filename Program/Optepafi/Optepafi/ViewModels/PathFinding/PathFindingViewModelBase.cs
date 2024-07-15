using System.Reactive;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

/// <summary>
/// Base class for every ViewModel corresponding to path finding session which can be set by <c>PathFindingSessionViewModel</c> as its current ViewModel.
/// 
/// It defines methods that are common to these ViewModels and are used specially by path finding session ViewModel.  
/// For more information on ViewModels in general see <see cref="ViewModelBase"/>.  
/// </summary>
public class PathFindingViewModelBase : ViewModelBase
{
    /// <summary>
    /// Command that is executed by path finding session ViewModel on currently used ViewModel when windows closed event occurs.
    /// 
    /// If ViewModel knows about some actions that should be taken before windows is closed, it should implement this command and execute them.  
    /// </summary>
    public virtual ReactiveCommand<Unit, Unit>? OnClosedCommand { get => null; }
}
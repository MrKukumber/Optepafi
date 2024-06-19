using System.Reactive;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingViewModelBase : ViewModelBase
{
    public virtual ReactiveCommand<Unit, Unit>? OnClosedCommand { get => null; }
}
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    public ReactiveCommand<Unit,Unit> ReturnCommand { get; }
    public ElevConfigViewModel()
    {
        ReturnCommand = ReactiveCommand.Create(() => { });
    }
    
    
}
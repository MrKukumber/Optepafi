using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using Optepafi.Models;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private ElevSource currentElevSource = new ElevSource();
    public ReactiveCommand<Unit, ElevSource> ReturnCommand { get; }
    public ElevConfigViewModel()
    {
        ReturnCommand = ReactiveCommand.Create(() => currentElevSource);
    }
    
}
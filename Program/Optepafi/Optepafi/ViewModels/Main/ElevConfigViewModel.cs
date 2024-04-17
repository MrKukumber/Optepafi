using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ModelViews;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private IElevSourceRep _currentElevSourceRep = null;//TODO:tuna bude nieco ine
    public ReactiveCommand<Unit, IElevSourceRep> ReturnCommand { get; }
    public ElevConfigViewModel()
    {
        ReturnCommand = ReactiveCommand.Create(() => _currentElevSourceRep);
    }
    
}

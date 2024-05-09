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
    private IElevDataSource _currentElevDataSource = null;//TODO:tuna bude nieco ine
    public ReactiveCommand<Unit, IElevDataSource> ReturnCommand { get; }
    public ElevConfigViewModel()
    {
        ReturnCommand = ReactiveCommand.Create(() => _currentElevDataSource);
    }
    
}

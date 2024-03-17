using System.Windows.Input;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    public ICommand ReturnCommand { get; }
    private MainWindowViewModel ParentMainWindow { get; }
    public ElevConfigViewModel(MainWindowViewModel parentMainWindow)
    {
        ParentMainWindow = parentMainWindow;
        ReturnCommand = ReactiveCommand.Create(() =>
            ParentMainWindow.CurrentViewModel = ParentMainWindow.MainSettings);
    }
    
    
}
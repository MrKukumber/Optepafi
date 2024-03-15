using System.Diagnostics;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    public MainWindowViewModel()
    {
        _currentViewModel = new MainMenuViewModel();
    }
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    
    
}
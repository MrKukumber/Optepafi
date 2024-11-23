using MapRepreViewer.ModelViews;
using ReactiveUI;

namespace MapRepreViewer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private MainWindowModelView _mainWindowModelView;

    public MapRepreViewingViewModel MapRepreViewing { get; } 
    
    public MainWindowViewModel(MainWindowModelView mainWindowModelView)
    {
        _mainWindowModelView = mainWindowModelView;
        MapRepreViewing = new MapRepreViewingViewModel(_mainWindowModelView.MapRepreViewing);
        CurrentViewModel = MapRepreViewing;
    }
    
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    private ViewModelBase _currentViewModel;
}
using System;
using System.Reactive;
using System.Reactive.Disposables;
using ExCSS;
using Optepafi.ModelViews.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

//TODO: comment
public class InitializationViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    private InitializationModelView _initializationModelView;
    
    public InitializationViewModel(InitializationModelView initializationModelView)
    {
        _initializationModelView = initializationModelView;
        Activator = new ViewModelActivator();

        InitializeCommand = ReactiveCommand.Create(() =>
        {
            _initializationModelView.InitializeResources();
        });
        
        this.WhenActivated(disposables =>
        {
            InitializeCommand.Execute().Subscribe().DisposeWith(disposables);
        });
    }
    
    public ReactiveCommand<Unit, Unit> InitializeCommand { get; }
}
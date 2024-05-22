using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding;

public partial class MapRepreCreatingWindow : ReactiveWindow<MapRepreCreatingWindowViewModel>
{
    public MapRepreCreatingWindow()
    {

        InitializeComponent();

        if (Design.IsDesignMode) return;

        // this.WhenActivated(action => action(ViewModel!.ReturnValueSet.Subscribe(proceed => Close(proceed))));
        this.WhenActivated(disposables =>
        {
            ViewModel!.CreateMapRepreCommand
                .Subscribe(isPossibleToContinue => Close(isPossibleToContinue))
                .DisposeWith(disposables);
            ViewModel!.ReturnCommand
                .Subscribe(isPossibleToContinue => Close(isPossibleToContinue))
                .DisposeWith(disposables);
        });
    }
}
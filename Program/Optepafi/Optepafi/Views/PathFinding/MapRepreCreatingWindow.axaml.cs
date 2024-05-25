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
    private void MapRepreCreatingWindow_OnClosed(object? sender, EventArgs e) { ViewModel!.OnClosedCommand.Execute(); }
}
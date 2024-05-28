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

        this.WhenActivated(d =>
        {
            d(ViewModel!.CreateMapRepreCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
            d(ViewModel!.ReturnCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
            d(ViewModel!.CancelMapRepreCreationCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
        });
    }
    private void MapRepreCreatingWindow_OnClosed(object? sender, EventArgs e) { ViewModel!.OnClosedCommand.Execute(); }
}
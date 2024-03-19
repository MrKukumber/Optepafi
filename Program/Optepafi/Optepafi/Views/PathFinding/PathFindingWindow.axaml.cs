using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;

namespace Optepafi.Views.PathFinding;

public partial class PathFindingWindow : ReactiveWindow<PathFindingWindowViewModel>
{
    public PathFindingWindow()
    {
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        ViewModel.OnClosingCommand.Execute(Unit.Default).Subscribe(closingRecommendation =>
        {
            switch (closingRecommendation)
            {
                case PathFindingWindowViewModel.ClosingRecommendation.CanClose:
                    e.Cancel = false;
                    break;
                //TODO:when added new values to ClosingRecommendation, handle them with new cases
            }
        });
    }

    private void Window_OnClosed(object? sender, EventArgs e)
    {
        ViewModel.OnClosedCommand.Execute(Unit.Default).Subscribe();
    }
}
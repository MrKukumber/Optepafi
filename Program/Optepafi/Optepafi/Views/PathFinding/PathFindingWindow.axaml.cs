using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding;


public partial class PathFindingWindow : ReactiveWindow<PathFindingSessionViewModel>
{
    public PathFindingWindow()
    {
        InitializeComponent();
        this.WhenActivated(action =>
            action(ViewModel!.PathFindingSettings.MapRepreCreationInteraction.RegisterHandler(DoShowMapRepreCreatingDialogAsync)));
    }

    private bool _alreadyAsked = false;
    private async void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_alreadyAsked) return;
        e.Cancel = true;
        bool close = await ViewModel!.OnClosingCommand.Execute();
        if (close)
        {
            _alreadyAsked = true;
            Close();
        }
    }

    private void Window_OnClosed(object? sender, EventArgs e)
    {
        ViewModel!.OnClosedCommand.Execute().Subscribe();
    }

    private async Task DoShowMapRepreCreatingDialogAsync(InteractionContext<MapRepreCreatingWindowViewModel, bool> interaction)
    {
        var dialog = new MapRepreCreatingWindow()
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }
}
using System;
using System.Reactive;
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

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        ViewModel.OnClosingCommand.Execute(Unit.Default).Subscribe(closingRecommendation =>
        {
            switch (closingRecommendation)
            {
                case PathFindingSessionViewModel.ClosingRecommendation.CanClose:
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

    private async Task DoShowMapRepreCreatingDialogAsync(
        InteractionContext<PFMapRepreCreatingModelView, bool>
            interaction)
    {
        var dialog = new MapRepreCreatingWindow()
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }
}
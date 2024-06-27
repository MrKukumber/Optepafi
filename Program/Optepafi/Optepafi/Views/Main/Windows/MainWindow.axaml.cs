using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.Main;
using ReactiveUI;
using Brushes = Avalonia.Media.Brushes;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace Optepafi.Views.Main.Windows;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            d(ViewModel!.MainSettings.ElevConfigInteraction.RegisterHandler(DoShowElevConfigAsync));
            d(ViewModel.YesNoInteraction.RegisterHandler(DoShowYesNoDialog));
        });
    }
    private async Task DoShowElevConfigAsync(InteractionContext<ElevConfigViewModel, ElevDataDistributionViewModel?> interaction)
    {
        AutoResetEvent returningEvent = new AutoResetEvent(false);
        ElevDataDistributionViewModel? result = null;
        var elevConfigView = new ElevConfigView
        {
            DataContext = interaction.Input
        };
        
        interaction.Input.ReturnCommand.Subscribe(elevDataDistr =>
        {
            result = elevDataDistr;
            returningEvent.Set();
        });
        
        Content = elevConfigView;
        await Task.Run(() => returningEvent.WaitOne()); 
        Content = ViewModel!.MainSettings;
        interaction.SetOutput(result);
    }

    private async Task DoShowYesNoDialog(InteractionContext<YesNoDialogWindowViewModel, bool> interaction)
    {
        var dialog = new YesNoDialogWindow()
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }

    private bool _alreadyAsked = false; 
    private async void MainWindow_OnClosing(object? sender, WindowClosingEventArgs e)
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
    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        ViewModel!.OnClosedCommand.Execute().Subscribe();
    }

}
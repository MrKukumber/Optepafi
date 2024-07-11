using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main.Windows;

/// <summary>
/// Main window of the application. This window is opened when application starts.
/// Also application shuts down when this window is closed.
/// This window is bounded to <see cref="MainWindowViewModel"/>. For more information on main windows functionality see documentation of this ViewModel.
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// When this component is activated besides initialization it registers handlers for all types of interaction, which are used in main window.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
        
        this.WhenActivated(d =>
        {
            d(ViewModel!.MainSettings.ElevConfigInteraction.RegisterHandler(DoShowElevConfigAsync));
            d(ViewModel.YesNoInteraction.RegisterHandler(DoShowYesNoDialog));
        });
    }
    /// <summary>
    /// Method for handling <c>ElevConfigInteraction</c>.
    /// It sets elevation data configuration as its content and awaits result of interaction with this configuration.
    /// After result is published it changes current content back to the main settings.
    /// Result is then set as an output of the interaction.
    /// </summary>
    /// <param name="interaction">Interaction to be handled.</param>
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

    /// <summary>
    /// Method for handling <c>YesNoInteraction</c>.
    /// It shows <see cref="YesNoDialogWindow"/> with request to user and awaits its response.
    /// Response is then set as result of interaction.
    /// </summary>
    /// <param name="interaction">Interaction to be handled.</param>
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
    /// <summary>
    /// Method for handling <c>OnClosing</c> event of this window.
    /// It designed in such way it could be cancelled by asynchronous dialog from user.
    /// </summary>
    /// <param name="sender">Sender of the <c>OnClosing</c> event.</param>
    /// <param name="e"><c>OnClosing</c> events arguments.</param>
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
    /// <summary>
    /// Method for handling <c>OnClosed</c> event of this window.
    /// </summary>
    /// <param name="sender">Sender of th <c>OnClosed</c> event.</param>
    /// <param name="e"><c>OnClosed</c> events arguments.</param>
    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        ViewModel!.OnClosedCommand.Execute().Subscribe();
    }

}
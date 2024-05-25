using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ViewModels;
using Optepafi.ViewModels.DataViewModels;
using Optepafi.ViewModels.Main;
using ReactiveUI;
using Brushes = Avalonia.Media.Brushes;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace Optepafi.Views.Main;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposeables =>
        {
            ViewModel!.MainSettings.ElevConfigInteraction.RegisterHandler(DoShowElevConfig).DisposeWith(disposeables);
            ViewModel.YesNoInteraction.RegisterHandler(DoShowYesNoDialog).DisposeWith(disposeables);
        });
    }

    private void DoShowElevConfig(InteractionContext<ElevConfigViewModel, ElevDataDistributionViewModel?> interaction)
    {
        Content = new ElevConfigView
        {
            DataContext = interaction.Input
        };

        interaction.Input.ReturnCommand.Subscribe(result =>
        {
            Content = ViewModel!.MainSettings;
            interaction.SetOutput(result);
        });

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
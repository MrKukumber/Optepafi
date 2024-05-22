using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Channels;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.ModelCreating;

namespace Optepafi.Views.ModelCreating;

public partial class ModelCreatingWindow : ReactiveWindow<ModelCreatingSessionViewModel>
{
    public ModelCreatingWindow()
    {
        InitializeComponent();
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
}
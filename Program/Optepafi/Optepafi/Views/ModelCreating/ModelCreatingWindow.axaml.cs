using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Channels;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.ModelCreating;

namespace Optepafi.Views.ModelCreating;

public partial class ModelCreatingWindow : ReactiveWindow<ModelCreatingWindowViewModel>
{
    public ModelCreatingWindow()
    {
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        ViewModel.OnClosingCommand.Execute(Unit.Default).Subscribe(closingRecommendation =>
        {
            switch (closingRecommendation)
            {
                case ModelCreatingWindowViewModel.ClosingRecommendation.CanClose:
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
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ViewModels;
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
        this.WhenActivated(action =>
            action(ViewModel.MainSettings.ElevConfigInteraction.RegisterHandler(ShowElevConfig)));
    }

    private void ShowElevConfig(InteractionContext<ElevConfigViewModel, IElevSource> interaction)
    {
        Content = new ElevConfigView
        {
            DataContext = interaction.Input
        };

        interaction.Input.ReturnCommand.Subscribe(result =>
        {
            Content = ViewModel.MainSettings;
            interaction.SetOutput(result);
        });

    }
}
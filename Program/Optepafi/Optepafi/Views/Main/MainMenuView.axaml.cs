using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using Optepafi.Views.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.Main;

public partial class MainMenuView : ReactiveUserControl<MainMenuViewModel>
{
    public MainMenuView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // ViewModel!.CreateModelCreatingSessionCommand.Subscribe(modelCreatingSession =>
                // {
                    // var newWindow = new ModelCreatingWindow
                    // {
                        // DataContext = modelCreatingSession,
                    // };
                    // newWindow.Show();
                // })
                // .DisposeWith(disposables);

            ViewModel!.CreatePathFindingSessionCommand.Subscribe(pathFindingSession =>
                {
                    var newWindow = new PathFindingWindow
                    {
                        DataContext = pathFindingSession
                    };
                    newWindow.Show();
                })
                .DisposeWith(disposables);
        });
    }
}
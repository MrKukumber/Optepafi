using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using ReactiveUI;
using PathFindingSessionWindow = Optepafi.Views.PathFinding.Windows.PathFindingSessionWindow;

namespace Optepafi.Views.Main;

/// <summary>
/// View of the main menu. It is the first View which is shown to the user after applications start.
/// For more information on main menus functionality see <see cref="MainMenuViewModel"/>.
/// </summary>
public partial class MainMenuView : ReactiveUserControl<MainMenuViewModel>
{
    /// <summary>
    /// When this component is activated, it subscribes session creating commands so it could create new session windows for them. 
    /// </summary>
    public MainMenuView()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
        
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
                    var newWindow = new PathFindingSessionWindow
                    {
                        DataContext = pathFindingSession
                    };
                    newWindow.Show();
                })
                .DisposeWith(disposables);
        });
    }
}
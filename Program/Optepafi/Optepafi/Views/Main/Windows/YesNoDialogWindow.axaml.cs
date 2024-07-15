using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main.Windows;

/// <summary>
/// Window which can be used for dialog with user whose response is either yes or no.
/// 
/// This window is meant to be used as dialog window.  
/// For more information on functionality of YesNoDialog see <see cref="YesNoDialogWindowViewModel"/>.  
/// </summary>
public partial class YesNoDialogWindow : ReactiveWindow<YesNoDialogWindowViewModel>
{
    /// <summary>
    /// On activation it subscribes to yes and no commands. The window is closed on their execution. 
    /// </summary>
    public YesNoDialogWindow()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;

        this.WhenActivated(disposables =>
        {
            ViewModel!.YesCommand.Subscribe(result => Close(result)).DisposeWith(disposables);
            ViewModel!.NoCommand.Subscribe(result => Close(result)).DisposeWith(disposables);
        });
    }
}
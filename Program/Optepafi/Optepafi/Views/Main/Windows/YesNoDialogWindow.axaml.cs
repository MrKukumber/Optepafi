using System;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main.Windows;

public partial class YesNoDialogWindow : ReactiveWindow<YesNoDialogWindowViewModel>
{
    public YesNoDialogWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel!.YesCommand.Subscribe(result => Close(result)).DisposeWith(disposables);
            ViewModel!.NoCommand.Subscribe(result => Close(result)).DisposeWith(disposables);
        });
    }
}
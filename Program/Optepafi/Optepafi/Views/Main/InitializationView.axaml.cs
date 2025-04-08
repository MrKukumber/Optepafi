using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main;

public partial class InitializationView : ReactiveUserControl<InitializationViewModel>
{
    public InitializationView()
    {
        InitializeComponent();

        this.WhenActivated(disposables => { });
    }
}
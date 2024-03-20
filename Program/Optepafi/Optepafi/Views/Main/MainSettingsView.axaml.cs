using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;

namespace Optepafi.Views.Main;

public partial class MainSettingsView : ReactiveUserControl<MainSettingsViewModel>
{
    public MainSettingsView()
    {
        InitializeComponent();
    }
}
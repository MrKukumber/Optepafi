using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.Models;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main;

public partial class MainSettingsView : ReactiveUserControl<MainSettingsViewModel>
{
    public MainSettingsView()
    {
        InitializeComponent();
    }

}
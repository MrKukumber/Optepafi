using System;
using System.Reactive.Disposables;
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

        if (Design.IsDesignMode) return;
        
        this.WhenActivated(disposables => ViewModel.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(newCulture =>
            {
                Assets.Localization.Local.Culture = newCulture;
                MainSettingsHeaderTextBlock.Text = Assets.Localization.Local.MainSettingsHeader;
            }).DisposeWith(disposables));

    }

}
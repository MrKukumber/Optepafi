using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main;

/// <summary>
/// View of the main settings.
/// For more information on main settings functionality see <see cref="MainSettingsViewModel"/>.
/// </summary>
public partial class MainSettingsView : ReactiveUserControl<MainSettingsViewModel>
{
    /// <summary>
    /// When component is activated, it subscribes current culture property, so it could change its appearance immediately when it changes.
    /// </summary>
    public MainSettingsView()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;
        
        // this.WhenActivated(disposables => ViewModel.WhenAnyValue(x => x.CurrentCulture)
            // .Subscribe(newCulture =>
            // {
                // MainSettingsHeaderTextBlock.Text = Assets.Localization.MainWindowLocal.Settings_MainSettingsHeader;
                // ElevConfigEntryButton.Content = Assets.Localization.MainWindowLocal.Settings_ElevConfigEntryButton;
                // MainMenuButton.Content = Assets.Localization.MainWindowLocal.Settings_MainMenuButton;
                // LocalizationTextBlock.Text = Assets.Localization.MainWindowLocal.Settings_LocalizationTextBlock;
            // }).DisposeWith(disposables));

    }

}
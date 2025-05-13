using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding.Windows;

/// <summary>
/// Dialog window used for showing map representation creation process.
/// 
/// This window is meant to be used as dialog window.  
/// Fo more information on map representation creation process see <see cref="MapRepreCreatingViewModel"/>.  
/// </summary>
public partial class MapRepreCreatingDialogWindow : ReactiveWindow<MapRepreCreatingViewModel>
{
    /// <summary>
    /// On activation of this component it subscribes to bunch of commands which are used for exiting of this dialog window.
    /// 
    /// It closes itself upon their execution.  
    /// </summary>
    public MapRepreCreatingDialogWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;

        this.WhenActivated(d =>
        {
            d(ViewModel!.CreateMapRepreCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
            d(ViewModel!.ReturnCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
            d(ViewModel!.CancelMapRepreCreationCommand.Subscribe(isPossibleToContinue => Close(isPossibleToContinue)));
            d(ViewModel.MainSettingsProvider.WhenAnyValue(x => x.CurrentCulture)
                .Subscribe(_ =>
                {
                    var currentContent = Content;
                    Content = null;
                    Content = currentContent;
                }));
        });
    }
    /// <summary>
    /// Method for handling <c>OnClosed</c> event of this window.
    /// </summary>
    /// <param name="sender">Sender of th <c>OnClosed</c> event.</param>
    /// <param name="e"><c>OnClosed</c> events arguments.</param>
    private void MapRepreCreatingWindow_OnClosed(object? sender, EventArgs e) { ViewModel!.OnClosedCommand?.Execute().Subscribe(); }
}
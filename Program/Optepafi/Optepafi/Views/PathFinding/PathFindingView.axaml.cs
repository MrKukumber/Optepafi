using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.Data;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.Utils;

namespace Optepafi.Views.PathFinding;

/// <summary>
/// View of path finding part of path finding mechanism.
/// 
/// For more information on path finding parts functionalities see <see cref="PathFindingViewModel"/>.  
/// </summary>
public partial class PathFindingView : ReactiveUserControl<PathFindingViewModel>
{
    public PathFindingView()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
    }

    /// <summary>
    /// Handles <c>OnPointerPressed</c> event of map graphics showing canvas.
    /// 
    /// It aggregates coordinates of pressed pointer and converts them to unit usable in applications logic.  
    /// Resulting coordinates are sent to <c>AddTrackPointCommand</c>s execution.  
    /// </summary>
    /// <param name="sender">Sender of <c>OnClick</c> event.</param>
    /// <param name="e"><c>OnClick</c> events arguments.</param>
    /// <exception cref="InvalidOperationException">Exception thrown when conversion from dip to micrometers did not run correctly.</exception>
    private async void MapGraphics_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var control = sender as Control;
        if (ViewModel!.IsShowingPathReport && control is not null) return;
        if (ViewModel.IsAcceptingTrack && control is not null)
        {
            var point = e.GetCurrentPoint(control);
            await ViewModel.AddTrackPointCommand.Execute((new CanvasCoordinate(
                MicrometersToDipConverter.Instance.ConvertBack(point.Position.X/ViewModel.GraphicsScale), 
                MicrometersToDipConverter.Instance.ConvertBack(point.Position.Y/ViewModel.GraphicsScale)), null));
        }
    }
}
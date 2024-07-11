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
            MicrometersToDipConverter converter = new MicrometersToDipConverter();
            if (converter.ConvertBack(point.Position.X/ViewModel.GraphicsScale) is int leftPos &&
                converter.ConvertBack(control.Height/ViewModel.GraphicsScale - point.Position.Y/ViewModel.GraphicsScale) is int bottomPos)
            {
                await ViewModel.AddTrackPointCommand.Execute((new CanvasCoordinate(leftPos, bottomPos), null));
            }
            else throw new InvalidOperationException("The conversion from dip to micrometers did not run correctly.");
        }
    }
}
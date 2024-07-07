using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Optepafi.Models.MapMan;
using Optepafi.ViewModels.Data;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.Utils;

namespace Optepafi.Views.PathFinding;

public partial class PathFindingView : ReactiveUserControl<PathFindingViewModel>
{
    public PathFindingView()
    {
        InitializeComponent();
    }

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
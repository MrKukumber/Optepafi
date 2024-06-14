using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Optepafi.Views.PathFinding;

public partial class PathFindingView : UserControl
{
    public PathFindingView()
    {
        InitializeComponent();
    }

    private void MapGraphics_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.GetCurrentPoint(sender as Control);
    }
}
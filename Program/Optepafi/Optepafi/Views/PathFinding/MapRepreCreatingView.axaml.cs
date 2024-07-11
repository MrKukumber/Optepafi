using Avalonia.Controls;
using Optepafi.ViewModels.PathFinding;

namespace Optepafi.Views.PathFinding;

/// <summary>
/// View for the map representation creating part of path finding mechanism.
/// For more information on map representation creation process see <see cref="MapRepreCreatingViewModel"/>.
/// </summary>
public partial class MapRepreCreatingView : UserControl
{
    public MapRepreCreatingView()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
    }
}
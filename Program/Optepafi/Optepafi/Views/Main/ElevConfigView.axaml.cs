using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.Main;

namespace Optepafi.Views.Main;

/// <summary>
/// View of the elevation data configuration mechanism.
/// 
/// For more information on this mechanism see <see cref="ElevConfigViewModel"/>.  
/// </summary>
public partial class ElevConfigView : UserControl
{
    public ElevConfigView()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
    }
    
    /// <summary>
    /// Convertor of regions presence to corresponding color.
    /// 
    /// It is used for painting a region for indication of its presence state.  
    /// </summary>
    public static FuncValueConverter<RegionViewModel.PresenceState, IBrush> PresenceToColorConverter { get; } =
        new(presence => presence switch
        {
            RegionViewModel.PresenceState.Downloaded => Brushes.GreenYellow,
            RegionViewModel.PresenceState.NotDownloaded => Brushes.White,
            RegionViewModel.PresenceState.IsDownloading =>Brushes.DarkOrange,
            RegionViewModel.PresenceState.IsDownloadingAsSubRegion => Brushes.Orange,
            RegionViewModel.PresenceState.IsDeleting => Brushes.Red, 
            _ => Brushes.Black
        });
    
}
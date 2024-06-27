using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.Main;
using ReactiveUI;

namespace Optepafi.Views.Main;

public partial class ElevConfigView : UserControl
{
    public Subject<ElevDataDistributionViewModel?> CompleteInteraction { get; } = new();
    public ElevConfigView()
    {
        InitializeComponent();
    }
    
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
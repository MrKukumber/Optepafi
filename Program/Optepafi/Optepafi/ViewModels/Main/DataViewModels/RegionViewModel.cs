using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using ReactiveUI;

namespace Optepafi.ViewModels.Main.DataViewModels;

public class RegionViewModel : ViewModelBase
{
    public Region Region { get; }
    public RegionViewModel(Region region)
    {
        Region = region;
        DownloadingCancellationTokenSource = new CancellationTokenSource();//Len aby bola inicializaovana, pre pouzitie sa aj tak bude vzdy dosadzovat nova instancia
        SubRegions = region.SubRegions.Select(subRegion => new SubRegionViewModel(this, subRegion)).ToArray(); //Pozor na cykly, zatial nebudem reisit
    }

    public string Name => Region.Name;
    public SubRegionViewModel[] SubRegions { get; }
    public List<GeoCoordinate>? Geometry => Region.Geometry;
    public enum PresenceState{Downloaded, IsDownloading, IsDeleting, NotDownloaded}
    private PresenceState _presence;
    public PresenceState Presence
    {
        get => _presence;
        set => this.RaiseAndSetIfChanged(ref _presence, value);
    }
    public CancellationTokenSource DownloadingCancellationTokenSource { get; set; }
}

public class SubRegionViewModel : RegionViewModel
{
    public SubRegionViewModel(RegionViewModel upperRegionViewModel, SubRegion subRegion) : base(subRegion)
    {
        UpperRegion = upperRegionViewModel;
    }
    public RegionViewModel UpperRegion { get; }
}

public class TopRegionViewModel : RegionViewModel
{
    public TopRegionViewModel(TopRegion topRegion) : base(topRegion){}
}
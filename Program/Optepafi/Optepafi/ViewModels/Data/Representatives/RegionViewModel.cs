using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Representatives;

/// <summary>
/// Wrapping ViewModel for <c>Region</c> type.
/// 
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.
/// </summary>
public abstract class RegionViewModel : WrappingDataViewModel<Region>
{
    
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override Region Data => Region;
    /// <summary>
    /// Coupled region instance.
    /// </summary>
    public Region Region { get; }

    /// <summary>
    /// Construction of new region ViewModel.
    /// 
    /// It is fine, that it does not initialize <c>SubRegions</c> property, because it is initialized in constructors of all successors of this type.
    /// </summary>
    /// <param name="region">Region to which will be coupled new ViewModel.</param>
    protected RegionViewModel(Region region) 
    {
        // SubRegions property will be initialized in successor constructor.
        Region = region;
        DownloadingCancellationTokenSource = new CancellationTokenSource(); //Instantiated only so it was not null. Nevertheless it will be reassigned every time it is used.
        Presence = region.IsDownloaded ? PresenceState.Downloaded : PresenceState.NotDownloaded;
    }

    public string Name => Region.Name;
    /// <summary>
    /// Array of sub-region ViewModels of this region.
    /// 
    /// For more information on subregions see <c>SubRegions</c> property of <see cref="Region"/>
    /// </summary>
    public SubRegionViewModel[] SubRegions { get; protected set; }
    
    /// <summary>
    /// List of <see cref="GeoCoordinate"/>s that defines the geometry of this region. It can be used for nicer presenting of regions to users.
    /// </summary>
    public List<GeoCoordinate>? Geometry => Region.Geometry; //TODO: namiesto GeoCoordinate dat CanvasCoordinate a prekonvertovat Region.Geometry na CanvasCoordinate
    
    /// <summary>
    /// Enumeration of states that describes presence of elevation data for this region. 
    /// </summary>
    public enum PresenceState{ NotDownloaded, IsDeleting, IsDownloading, IsDownloadingAsSubRegion, Downloaded }
    
    /// <summary>
    /// Property which describes in which state are elevation data for this region.
    /// </summary>
    public PresenceState Presence
    {
        get => _presence;
        set => this.RaiseAndSetIfChanged(ref _presence, value);
    }
    private PresenceState _presence;
    
    /// <summary>
    /// Cancellation token source used for cancelling of elevation data download.
    /// 
    /// Each region has its own variable for this source. When user does want to cancel elev. data download, this source fires request for cancellation.  
    /// It should be reassigned every time when download of data is initiated so it could be correctly used for firing cancellation request.  
    /// </summary>
    public CancellationTokenSource DownloadingCancellationTokenSource { get; set; }


    public void UpdatePresence() => Presence = Region.IsDownloaded ? PresenceState.Downloaded : PresenceState.NotDownloaded;
}

/// <summary>
/// Wrapping ViewModel for <c>SuRegion</c> type.
/// 
/// It inherits from RegionViewModel.  
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
public class SubRegionViewModel : RegionViewModel
{
    /// <summary>
    /// Construction of new sub-region ViewModel.
    /// 
    /// It prevents infinite loop caused by cyclical reference of sub-regions.  
    /// </summary>
    /// <param name="upperRegionViewModel">View model of the upper region of sub-region coupled to this ViewModel.</param>
    /// <param name="subRegion">Region to which will be coupled new ViewModel .</param>
    /// <param name="alreadyVisitedRegions">Stack of previously visited regions for which their ViewModels were created. It is used for infinite loop prevention.</param>
    public SubRegionViewModel(RegionViewModel upperRegionViewModel, SubRegion subRegion, Stack<Region> alreadyVisitedRegions) : base(subRegion)
    {
        UpperRegion = upperRegionViewModel;
        SubRegions = subRegion.SubRegions
            .Where(sr => !alreadyVisitedRegions.Contains(sr))
            .Select(sr =>
            {
                alreadyVisitedRegions.Push(sr);
                var subRegionViewModel = new SubRegionViewModel(this, sr, alreadyVisitedRegions);
                alreadyVisitedRegions.Pop();
                return subRegionViewModel;
            }).ToArray();
    }
    
    /// <summary>
    /// ViewModel of upper region of coupled sub-region.
    /// </summary>
    public RegionViewModel UpperRegion { get; }
}

/// <summary>
/// Wrapping ViewModel for <c>TopRegion</c> type.
/// 
/// It inherits from RegionViewModel.  
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
public class TopRegionViewModel : RegionViewModel
{
    /// <summary>
    /// Construction of new top region ViewModel.
    /// </summary>
    /// <param name="topRegion">Region to which will be coupled new ViewModel .</param>
    public TopRegionViewModel(TopRegion topRegion) : base(topRegion)
    {
        SubRegions = topRegion.SubRegions
            .Select(subRegion => new SubRegionViewModel(this, subRegion, new Stack<Region>()) ).ToArray();
    }
}
using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.Simulating;

/// <summary>
/// Demonstrating subregion used for presenting application functionalities.
/// 
/// For more information on subregions see <see cref="ISubRegion"/>.  
/// </summary>
public class SubRegion1 : ISubRegion
{
    /// <inheritdoc cref="IRegion.Name"/>
    public string Name => "Subregion 1";

    public SubRegion1(TopRegion0 upperRegion)
    {
        UpperRegion = upperRegion;
        upperRegion.SubRegionsSet.Add(this);
    }
    /// <inheritdoc cref="IRegion.SubRegions"/> 
    public IReadOnlyCollection<ISubRegion> SubRegions { get; } = new HashSet<ISubRegion>();
    
    /// <inheritdoc cref="ISubRegion.UpperRegion"/> 
    public IRegion UpperRegion { get; }
    
    public bool IsDownloaded { get; set; }
    
}
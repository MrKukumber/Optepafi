using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.Simulating;

/// <summary>
/// Demonstration top region used for presenting application functionalities.
/// 
/// For more information on top regions see <see cref="ITopRegion"/>.  
/// </summary>
public class TopRegion0 : ITopRegion
{
    
    /// <inheritdoc cref="IRegion.Name"/>
    public string Name => "Top region 0";
    
    public HashSet<ISubRegion> SubRegionsSet { get; set; } = new();

    /// <inheritdoc cref="IRegion.SubRegions"/> 
    public IReadOnlyCollection<ISubRegion> SubRegions => SubRegionsSet; 
    
    public bool IsDownloaded { get; set; }
}
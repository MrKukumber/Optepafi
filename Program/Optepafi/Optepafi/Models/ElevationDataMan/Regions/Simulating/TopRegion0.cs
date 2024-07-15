using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.Simulating;

/// <summary>
/// Demonstration top region used for presenting application functionalities.
/// 
/// For more information on top regions see <see cref="TopRegion"/>.  
/// </summary>
public class TopRegion0 : TopRegion
{
    
    /// <inheritdoc cref="Region.Name"/>
    public override string Name => "Top region 0";
    
    /// <inheritdoc cref="Region.SubRegions"/> 
    public override HashSet<SubRegion> SubRegions { get; } = new();
}
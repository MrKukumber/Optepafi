using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.Simulating;

/// <summary>
/// Demonstrating subregion used for presenting application functionalities.
/// For more information on subregions see <see cref="SubRegion"/>.
/// </summary>
public class SubRegion1 : SubRegion
{
    /// <inheritdoc cref="Region.Name"/>
    public override string Name => "Subregion 1";

    public SubRegion1(Region upperRegion)
    {
        UpperRegion = upperRegion;
        upperRegion.SubRegions.Add(this);
    }
    /// <inheritdoc cref="Region.SubRegions"/> 
    public override HashSet<SubRegion> SubRegions { get; } = new();
    
    /// <inheritdoc cref="SubRegion.UpperRegion"/> 
    public override Region UpperRegion { get; }
}
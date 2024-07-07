using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.NotReal;

public class SoNotRealSubRegion : SubRegion
{
    
    public override string Name => "So not real region";

    public SoNotRealSubRegion(Region upperRegion)
    {
        UpperRegion = upperRegion;
        upperRegion.SubRegions.Add(this);
    }
    public override HashSet<SubRegion> SubRegions { get; } = new();
    public override Region UpperRegion { get; }
}
using System.Collections;
using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.NotReally;

public class SoNotRealRegion : SubRegion
{
    
    public override string Name => "So not real region";

    public SoNotRealRegion(Region upperRegion)
    {
        UpperRegion = upperRegion;
        upperRegion.SubRegions.Add(this);
    }
    public override HashSet<SubRegion> SubRegions { get; } = new();
    public override Region UpperRegion { get; }
}
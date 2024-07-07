using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.NotReal;

public class LittleLessNotRealSubReagion : SubRegion
{
    public override string Name => "Little less not real region";

    public LittleLessNotRealSubReagion(Region upperRegion)
    {
        UpperRegion = upperRegion;
        upperRegion.SubRegions.Add(this);
    }
    public override HashSet<SubRegion> SubRegions { get; } = new();
    public override Region UpperRegion { get; }
}
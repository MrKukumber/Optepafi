using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.NotReal;

public class NotRealTopRegion : TopRegion
{
    public override string Name => "Not real region";
    public override HashSet<SubRegion> SubRegions { get; } = new();
}
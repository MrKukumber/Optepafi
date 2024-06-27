using System.Collections.Generic;

namespace Optepafi.Models.ElevationDataMan.Regions.NotReally;

public class NotRealRegion : TopRegion
{
    public override string Name => "Not real region";
    public override HashSet<SubRegion> SubRegions { get; } = new();
}
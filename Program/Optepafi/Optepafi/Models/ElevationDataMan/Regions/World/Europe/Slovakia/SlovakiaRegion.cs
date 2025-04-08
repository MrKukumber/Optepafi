using System.Collections.Generic;
using Optepafi.Models.Utils;

namespace Optepafi.Models.ElevationDataMan.Regions.World.Europe.Slovakia;

public abstract class SlovakiaRegion : IRegion
{
    public string Name { get; } = "Slovakia";
    public List<GeoCoordinates>? Geometry { get => null; }
    public abstract IReadOnlyCollection<ISubRegion> SubRegions { get; }
    public bool IsDownloaded { get; set; }
}
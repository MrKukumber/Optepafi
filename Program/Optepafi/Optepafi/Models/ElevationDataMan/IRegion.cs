using System;
using System.Collections.Generic;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.ElevationDataMan;

public interface IRegion
{
    public string Name { get; }
    public ISubRegion[] SubRegions { get; }
    public List<GeoCoordinate>? Geometry { get => null; }
    public bool IsDownloaded { get; set; }   
}

public interface ISubRegion : IRegion
{
    public IRegion UppreRegion { get; }
}
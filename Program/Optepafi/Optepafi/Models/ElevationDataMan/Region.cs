using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.ElevationDataMan;

public abstract class Region 
{
    public abstract string Name { get; }
    public abstract SubRegion[] SubRegions { get; }
    public virtual List<GeoCoordinate>? Geometry { get => null; }
    public bool IsDownloaded { get; set; }
    
}

public abstract class SubRegion : Region
{
    public abstract Region UpperRegion { get; }
}

public abstract class TopRegion : Region { }
using System.Collections.Generic;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.ElevationDataMan.Regions;

/// <summary>
/// This class represents one whole region, that is used by elevation data distributions for organizing of their data management.
/// Each elevation data distribution defines its own set of these regions which it supports.
/// Each region can contain list of its subRegions for more detailed area definition. When some region is operated with, his subregions and upper region should be took into consideration too.
/// Moreover it can indicate whether it is downloaded or not.????
/// </summary>
//TODO: premysliet ci vlastnost IsDownloaded by mala byt sucastou tohto konstruktu
public abstract class Region 
{
    public abstract string Name { get; }
    /// <summary>
    /// List of sub-regions of this region. When some operation with this region is done, they should be took into consideration too.
    /// </summary>
    public abstract HashSet<SubRegion> SubRegions { get; }
    /// <summary>
    /// List of <see cref="GeoCoordinate"/>s that defines the geometry of this region. It can be used for nicer presenting of regions to users.
    /// </summary>
    public virtual List<GeoCoordinate>? Geometry { get => null; }
    public bool IsDownloaded { get; set; }
}

/// <summary>
/// This class represent sub-region. It is specific type of region that also includes reference to its upper region.
/// For more information about regions see <see cref="Region"/>.
/// </summary>
public abstract class SubRegion : Region
{
    /// <summary>
    /// Referencej to the upper region of this sub-region. When some operation with sub-region is done, it should be took into consideration too.
    /// </summary>
    public abstract Region UpperRegion { get; }
}
/// <summary>
/// This class represents top level regions. They can not be used as sub regions of other regions. They are mainly used by elevation data distributions in list of supported regions.
/// For more information about regions see <see cref="Region"/>.
/// </summary>
public abstract class TopRegion : Region { }
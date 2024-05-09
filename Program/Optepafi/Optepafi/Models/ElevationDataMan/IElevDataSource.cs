using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.ElevationDataMan;

public interface IElevDataSource
{
    public ISet<IRegion> AllRegions();
    //return true, if download or removal were successful
    //throw ArgumentException, if given region is not region from AllRegions
    public bool Download(IRegion region);
    public bool Remove(IRegion region);
    
    public bool AreElevDataDownloadedFor(IGeoReferencedMap map);
    public IElevData? GetElevDataFor(IGeoReferencedMap map);
}
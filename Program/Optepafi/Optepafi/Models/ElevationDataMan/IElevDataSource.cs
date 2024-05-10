using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan;

public interface IElevDataSource
{
    public ISet<IRegion> AllRegions();
    //return true, if download or removal were successful
    //throw ArgumentException, if given region is not region from AllRegions
    public bool Download(IRegion region);
    public bool Remove(IRegion region);
    
    public bool AreElevDataDownloadedFor(IDirectlyAreaQueryableMap map);
    public bool AreElevDataDownloadedFor(IByReferenceAreaQueryableMap map);
    public bool AreElevDataDownloadedFor(IMostNSWECoordQueryableGeoLocMap map);
    public bool AreElevDataDownloadedFor(IMostNSWECoordQueryableGeoRefMap map);
    public IElevData? GetElevDataFor(IDirectlyAreaQueryableMap map);
    public IElevData? GetElevDataFor(IByReferenceAreaQueryableMap map);
    public IElevData? GetElevDataFor(IMostNSWECoordQueryableGeoLocMap map);
    public IElevData? GetElevDataFor(IMostNSWECoordQueryableGeoRefMap map);
}
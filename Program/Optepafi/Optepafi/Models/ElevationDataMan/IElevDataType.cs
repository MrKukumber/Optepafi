using System.Collections.Generic;
using System.Net;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan;

public interface IElevDataType
{
    
    public string Name { get; }
    public IReadOnlySet<TopRegion> AllTopRegions { get; }
    //throw ArgumentException, if given region is not region from AllRegions
    public void Remove(Region region);
    
    public bool AreElevDataDownloadedFor(IDirectlyAreaQueryableMap map);
    public bool AreElevDataDownloadedFor(IByReferenceAreaQueryableMap map);
    public bool AreElevDataDownloadedFor(IMostNSWECoordQueryableGeoLocMap map);
    public bool AreElevDataDownloadedFor(IMostNSWECoordQueryableGeoRefMap map);
    public IElevData? GetElevDataFor(IDirectlyAreaQueryableMap map);
    public IElevData? GetElevDataFor(IByReferenceAreaQueryableMap map);
    public IElevData? GetElevDataFor(IMostNSWECoordQueryableGeoLocMap map);
    public IElevData? GetElevDataFor(IMostNSWECoordQueryableGeoRefMap map);
}

public interface ICredentialsNotRequiringElevDataType : IElevDataType
{
    
    //throw ArgumentException, if given region is not region from AllRegions
    public ElevDataManager.DownloadingResult Download(Region region, CancellationToken? cancellationToken);
}
public interface ICredentialsRequiringElevDataType : IElevDataType
{
    
    //throw ArgumentException, if given region is not region from AllRegions
    public ElevDataManager.DownloadingResult Download(Region region, NetworkCredential credential, CancellationToken? cancellationToken);
}

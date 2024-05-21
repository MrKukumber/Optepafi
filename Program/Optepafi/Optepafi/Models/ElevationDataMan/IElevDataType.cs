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
    
    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IGeoLocatedMap map, CancellationToken? cancellationToken);
    
    //throw exception, if elev data are not obtainable for map
    public IElevData GetElevDataFor(IGeoLocatedMap map, CancellationToken? cancellationToken);
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

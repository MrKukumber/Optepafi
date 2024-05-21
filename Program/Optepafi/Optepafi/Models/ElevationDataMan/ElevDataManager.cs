using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan;

public class ElevDataManager
{
    public static ElevDataManager Instance { get; } = new();
    private ElevDataManager(){}

    public IReadOnlySet<IElevDataSource> ElevDataSources { get; } =
        ImmutableHashSet.Create<IElevDataSource>( /*TODO: add elevation data sources*/); //TODO: este premysliet ako reprezentovat, mozno skor nejakym listom koli poradiu
    
    
    public enum DownloadingResult {Downloaded, Canceled, UnableToDownload}

    public DownloadingResult DownloadRegion(ICredentialsNotRequiringElevDataType elevDataType, Region region,
        CancellationToken? cancellationToken)
    {
        return elevDataType.Download(region, cancellationToken);
    }
    
    public DownloadingResult DownloadRegion(ICredentialsRequiringElevDataType elevDataType, Region region, NetworkCredential credential,
        CancellationToken? cancellationToken)
    {
        return elevDataType.Download(region, credential, cancellationToken);
    }
    
    public void RemoveRegion(IElevDataType elevDataType, Region region)
    {
        elevDataType.Remove(region);
    }


    public enum ElevDataObtainability {Obtainable, ElevDataNotPresent, NotSupportedMap}
    public ElevDataObtainability AreElevDataOfTypeObtainableFor(IGeoLocatedMap map, IElevDataType elevDataType, CancellationToken? cancellationToken = null)
    {
        // return map switch
        // {
            // IDirectlyAreaQueryableMap m => elevDataType.AreElevDataObtainableFor(m),
            // IByReferenceAreaQueryableMap m => elevDataType.AreElevDataObtainableFor(m),
            // IMostNSWECoordQueryableGeoLocMap m => elevDataType.AreElevDataObtainableFor(m),
            // IMostNSWECoordQueryableGeoRefMap m => elevDataType.AreElevDataObtainableFor(m),
            // _ => false
        // };
        return elevDataType.AreElevDataObtainableFor(map, cancellationToken);
    }

    public IElevData GetElevDataOfTypeFor(IGeoLocatedMap map, IElevDataType elevDataType, CancellationToken? cancellationToken = null)
    {
        
        // return map switch
        // {
            // IDirectlyAreaQueryableMap m => elevDataType.GetElevDataFor(m),
            // IByReferenceAreaQueryableMap m => elevDataType.GetElevDataFor(m),
            // IMostNSWECoordQueryableGeoLocMap m => elevDataType.GetElevDataFor(m),
            // IMostNSWECoordQueryableGeoRefMap m => elevDataType.GetElevDataFor(m),
            // _ => throw new ArgumentException("Unsupported map type")
        // };
        return elevDataType.GetElevDataFor(map, cancellationToken);
    }

}
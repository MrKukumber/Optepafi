using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;

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

}
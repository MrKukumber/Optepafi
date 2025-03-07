using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.ElevSources;
using Optepafi.Models.ElevationDataMan.ElevSources.NotSufficient;
using Optepafi.Models.ElevationDataMan.ElevSources.Simulating;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan;

/// <summary>
/// Singleton class used for managing elevation data used as supplementary source for creation of map representations.
/// 
/// It contains set of all usable elevation data sources. It represents main channel between operations with elevation data and applications logic (ModelViews/ViewModels).  
/// It implements supporting methods for work with elevation data. Elevation data should be managed preferably through this class.  
/// All operations provided by this class are thread safe as long as same arguments are not used concurrently multiple times. That means elevation data sources provided and used by this class should be prepared for asynchronous usage.  
/// </summary>
public class ElevDataManager
{
    public static ElevDataManager Instance { get; } = new();
    private ElevDataManager(){}

    /// <summary>
    /// Set of usable elevation data sources.
    /// </summary>
    public IReadOnlySet<IElevDataSource> ElevDataSources { get; } =
        ImmutableHashSet.Create<IElevDataSource>(SimulatingElevDataSource.Instance, NotSufficientElevDataSource.Instance); //TODO: este premysliet ako reprezentovat, mozno skor nejakym listom koli poradiu
    
    public enum DownloadingResult {Downloaded, Canceled, UnableToDownload, WrongCredentials}

    /// <summary>
    /// Method for downloading specified region of specified elevation data distribution from elevation data source with no need of credentials for accessing those data.
    /// </summary>
    /// <param name="elevDataDistribution">Elevation data distribution that will provide elevation data for given region.</param>
    /// <param name="region">Region that should be downloaded.</param>
    /// <param name="cancellationToken">Token for cancelling loading of data when they are not needed anymore.</param>
    /// <returns>Result of downloading.</returns>
    public DownloadingResult DownloadRegion(ICredentialsNotRequiringElevDataDistribution elevDataDistribution, Region region,
        CancellationToken? cancellationToken = null)
    {
        return elevDataDistribution.Download(region, cancellationToken);
    }
    
    /// <summary>
    /// Method for downloading specified region of specified elevation data distribution from elevation data source with requiring credentials for accessing those data.
    /// </summary>
    /// <param name="elevDataDistribution">Elevation data distribution that will provide elevation data for given region.</param>
    /// <param name="region">Region that should be downloaded.</param>
    /// <param name="credential">Credentials for accessing the elevation data.</param>
    /// <param name="cancellationToken">Token for cancelling loading of data when they are not needed anymore.</param>
    /// <returns>Result of downloading.</returns>
    public DownloadingResult DownloadRegion(ICredentialsRequiringElevDataDistribution elevDataDistribution, Region region, NetworkCredential credential,
        CancellationToken? cancellationToken)
    {
        return elevDataDistribution.Download(region, credential, cancellationToken);
    }
    
    /// <summary>
    /// Method for removal of specified downloaded region of specified elevation data distribution.
    /// </summary>
    /// <param name="elevDataDistribution">Eevation data distribution whose data should be removed.</param>
    /// <param name="region">Region whose data should be removed.</param>
    public void RemoveRegion(IElevDataDistribution elevDataDistribution, Region region)
    {
        elevDataDistribution.Remove(region);
    }

    public enum ElevDataObtainability {Obtainable, ElevDataNotPresent, NotSupportedMap, Cancelled}
    /// <summary>
    /// Method which tests whether elevation data for given geo located map are acquirable.
    ///
    /// If this method returns positive response, it will be ensured, that elevation data for provided map will be guaranteed.  
    /// This test can start asynchronous retrieving and preparing of data so they were ready for their retrieval. 
    /// </summary>
    /// <param name="map">Map for which test for elevation data obtainability is requested.</param>
    /// <param name="elevDataDistribution">Elevation data distribution asked for its ability to deliver requested data.</param>
    /// <param name="cancellationToken">Token for cancellation of process.</param>
    /// <returns></returns>
    public ElevDataObtainability AreElevDataFromDistObtainableFor(IAreaQueryableMap map, IElevDataDistribution elevDataDistribution, CancellationToken? cancellationToken = null)
    {
        return elevDataDistribution.AreElevDataObtainableFor(map, cancellationToken);
    }

    /// <summary>
    /// Method for retrieving of elevation data for provided map of provided elevation data distribution.
    /// 
    /// Calling this method should be preceded by calling the <see cref="AreElevDataFromDistObtainableFor"/> method for testing obtainability of required elevation data.   
    /// In case test has a positive result, it is ensured that required data will be provided. On the other hand call of this method could throw an invalid operation exception.  
    /// Elevation data sources should be prepared to provide data asynchronously, so this method can be called from multiple threads simultaneously.  
    /// </summary>
    /// <param name="map">Map for which which elevation data are requested.</param>
    /// <param name="elevDataDistribution">Elevation data distribution asked for providing elevation data.</param>
    /// <param name="cancellationToken">Token for cancellation of elevation data delivery.</param>
    /// <returns>Elevation data object which is able to provide required data.</returns>
    public IElevData GetElevDataFromDistFor(IAreaQueryableMap map, IElevDataDistribution elevDataDistribution, CancellationToken? cancellationToken = null)
    {
        return elevDataDistribution.GetElevDataFor(map, cancellationToken);
    }
    
}
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan.Distributions;

/// <summary>
/// Represents elevation data distribution provided by some source of data. It provides methods for downloading, removal and retrieval of by it represented elevation data.
/// 
/// For accessing downloading of data it is necessary to implement one of its descendant: <see cref="ICredentialsNotRequiringElevDataDistribution"/>  or <see cref="ICredentialsRequiringElevDataDistribution"/>.  
/// One represents elevation data distributions, for which it is not required to send credentials for accessing data form server. On the contrary, the second one represents those data types, which requires them.
/// 
/// Downloading and removal of data is done by regions. Each elevation data distribution defines its own set of regions which it supports.  
/// Elevation data are mostly loaded in bigger chunks called cells, so it is responsibility of each elevation data distribution to correctly manage these resources.  
/// Before each retrieval of the data it should be checked, if they are obtainable. If check was not done, retrieving of data could result in thrown exceptions. On the other hand, if check had positive result, successful retrieval should be ensured.
/// 
/// All operations provided by <c>IElevDataDistribution</c> should be made thread safe, so the retrieval of elevation data could be provided asynchronously.  
/// Preferred way to interact with distributions is through <see cref="ElevDataManager"/>.
/// </summary>
public interface IElevDataDistribution
{
    public string Name { get; }
    
    /// <summary>
    /// Set of all <see cref="TopRegion"/>s that are provided by this data distribution.
    /// </summary>
    public IReadOnlySet<TopRegion> AllTopRegions { get; }
    
    /// <summary>
    /// Method used for removing of defined region. If given region is not provided by this distribution, exception is thrown.
    /// </summary>
    /// <param name="region">Region to be removed.</param>
    // TODO: ci by som nemal odstranovanie a downloadovanie genericke cez regiony aby sa nemohlo stat ze sa do tejto metody lozi nespravny typ regionu a zaroven to zaruci, ze si tieto regiony nebudu musiet distribucie castovat
    public void Remove(Region region);
    
    /// <summary>
    /// Testing method for receiving of information whether for provided map there are available corresponding elevation data in this data distribution.
    /// 
    /// If this method returns positive response, it should be ensured that in the near future will be questioned data ready for delivery.  
    /// Each implementation of methods should support at least maps, which implement interface <see cref="IMostNSWECoordQueryableGeoLocMap"/> or interface <see cref="IMostNSWECoordQueryableGeoRefMap"/>.  
    /// </summary>
    /// <param name="map">Map for which test for elevation data obtainability is requested.</param>
    /// <param name="cancellationToken">Token for cancellation of testing process.</param>
    /// <returns>Resulting obtainability of elevation data for map.</returns>
    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IAreaQueryableMap map, CancellationToken? cancellationToken);
    
    /// <summary>
    /// Method for retrieving of elevation data for provided map.
    /// 
    /// Calling ths method should be preceded by calling the <see cref="AreElevDataObtainableFor"/> method for testing obtainability of required elevation data.  
    /// In case test has a positive result, it is ensured that required data will be provided. On the other hand call of this method could throw an invalid operation exception.  
    /// Elevation data distributions should be prepared to provide data asynchronously, so this method can be called from multiple threads simultaneously.  
    /// </summary>
    /// <param name="map">Map for which elevation data are requested.</param>
    /// <param name="cancellationToken">Token for cancellation of elevation data delivery.</param>
    /// <returns>Elevation data object which is able to provide required data.</returns>
    public IElevData GetElevDataFor(IAreaQueryableMap map, CancellationToken? cancellationToken);
}


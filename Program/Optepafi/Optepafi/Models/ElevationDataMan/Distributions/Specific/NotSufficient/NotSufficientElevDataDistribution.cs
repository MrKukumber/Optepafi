using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;

namespace Optepafi.Models.ElevationDataMan.Distributions.Specific.NotSufficient;

/// <summary>
/// Elevation data distribution which never contains sufficient elevation data for provided map. It defines no regions what so ever.
/// 
/// This type is just demonstrative elevation data distribution for presenting application functionality.  
/// For more information on elevation data distributions which does not require authorization see <see cref="ICredentialsNotRequiringElevDataDistribution"/>.  
/// </summary>
public class NotSufficientElevDataDistribution : ICredentialsNotRequiringElevDataDistribution
{
    
    public static NotSufficientElevDataDistribution Instance { get; } = new();
    private NotSufficientElevDataDistribution()
    {
        AllTopRegions = new HashSet<ITopRegion>();
    }

    
    /// <inheritdoc cref="IElevDataDistribution.Name"/>
    public string Name => "Not sufficient elevation data distribution";
    
    /// <inheritdoc cref="IElevDataDistribution.AllTopRegions"/> 
    public IReadOnlyCollection<ITopRegion> AllTopRegions { get; }
    
    /// <inheritdoc cref="IElevDataDistribution.Remove"/>
    /// <remarks>
    /// Because distribution does not define any region, it does not have to bother with their removal.
    /// </remarks>
    public void Remove(IRegion region) { }

    /// <inheritdoc cref="IElevDataDistribution.AreElevDataObtainableFor"/>
    /// <remarks>
    /// Elevation data for given map are never available.
    /// </remarks>
    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return ElevDataManager.ElevDataObtainability.Cancelled;
        return ElevDataManager.ElevDataObtainability.ElevDataNotPresent;
    }

    /// <inheritdoc cref="IElevDataDistribution.GetElevDataFor"/>
    public IElevData GetElevDataFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        return new ElevData();
    }

    /// <inheritdoc cref="ICredentialsNotRequiringElevDataDistribution.Download"/>
    /// <remarks>
    /// Distribution is not able to download any region because it does not define any. 
    /// </remarks>
    public ElevDataManager.DownloadingResult Download(IRegion region, CancellationToken? cancellationToken)
    {
        return ElevDataManager.DownloadingResult.UnableToDownload;
    }
    
    /// <inheritdoc cref="IElevDataDistribution.Initialize"/>
    public void Initialize(){}
    
    /// <summary>
    /// Represents elevation data, which does not contain any information.
    /// </summary>
    private class ElevData : IElevData
    {
        
        /// <inheritdoc cref="IElevData.GetElevation(GeoCoordinates)"/>
        public double? GetElevation(GeoCoordinates coordinates)
        {
            return null;
        }

        /// <inheritdoc cref="IElevData.GetElevation(MapCoordinates,GeoCoordinates)"/>
        public double? GetElevation(MapCoordinates coordinates, GeoCoordinates geoReference, int scale)
        {
            return null;
        }
    }
}
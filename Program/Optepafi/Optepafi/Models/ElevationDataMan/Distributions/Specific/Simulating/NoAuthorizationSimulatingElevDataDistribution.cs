using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.ElevationDataMan.Regions.Simulating;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;

namespace Optepafi.Models.ElevationDataMan.Distributions.Specific.Simulating;

/// <summary>
/// Elevation data source which simulates work of authorization not requiring elevation data distribution.
/// 
/// This type is just demonstrative data source for presenting application functionality.  
/// For more information on elevation data sources which does not require authorization for accessing data see <see cref="ICredentialsNotRequiringElevDataDistribution"/>.  
/// </summary>
public class NoAuthorizationSimulatingElevDataDistribution : ICredentialsNotRequiringElevDataDistribution
{
    
    public static NoAuthorizationSimulatingElevDataDistribution Instance { get; } = new();
    
    /// <summary>
    /// Initialize three demonstrating regions. One top region and two subregions.
    /// </summary>
    private NoAuthorizationSimulatingElevDataDistribution()
    {
        TopRegion0 topRegion = new TopRegion0()
        {
            IsDownloaded = false
        };
        new SubRegion2(topRegion)
        {
            IsDownloaded = true
        };
        new SubRegion1(topRegion)
        {
            IsDownloaded = false
        };
        AllTopRegions = new HashSet<ITopRegion>{ topRegion };
    }
    
    /// <inheritdoc cref="IElevDataDistribution.Name"/>
    public string Name => "No authorization simulating elevation data distribution";
    
    /// <inheritdoc cref="IElevDataDistribution.AllTopRegions"/>
    public IReadOnlyCollection<ITopRegion> AllTopRegions { get; }
    
    /// <inheritdoc cref="IElevDataDistribution.Remove"/>
    public void Remove(IRegion region)
    {
        Thread.Sleep(500); //Lot of work with removing of regions data
        RemoveRecursivelySubRegions(region);
        SetRecursivelyUpperRegionsToNotDownloaded(region);
    }
    private void RemoveRecursivelySubRegions(IRegion region)
    {
        region.IsDownloaded = false;
        foreach (var subRegion in region.SubRegions)
        {
            RemoveRecursivelySubRegions(subRegion);
        }
    }
    private void SetRecursivelyUpperRegionsToNotDownloaded(IRegion region)
    {
        region.IsDownloaded = false;
        if(region is ISubRegion subRegion) SetRecursivelyUpperRegionsToNotDownloaded(subRegion.UpperRegion);
    }

    
    /// <inheritdoc cref="IElevDataDistribution.AreElevDataObtainableFor"/>
    /// <remarks>
    /// Elevation data of this distribution are always available for any provided map.
    /// </remarks>
    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return ElevDataManager.ElevDataObtainability.Cancelled;
        return ElevDataManager.ElevDataObtainability.Obtainable;
    }

    
    /// <inheritdoc cref="IElevDataDistribution.GetElevDataFor"/>
    public IElevData GetElevDataFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        return new ElevData();
    }

    /// <inheritdoc cref="ICredentialsNotRequiringElevDataDistribution.Download"/>
    /// <remarks>
    /// This method simulates downloading of provided region.
    /// It tries to download all subregions with small probability of unsuccessful download.
    /// Responds to cancellation of downloading.
    /// </remarks>
    public ElevDataManager.DownloadingResult Download(IRegion region, CancellationToken? cancellationToken)
    {
        Random rnd = new Random();
        List<IRegion> subRegionsWhichWereSuccessfulyDownloaded = new();
        foreach (var subRegion in region.SubRegions)
        {
            if (!subRegion.IsDownloaded)
            {
                Thread.Sleep(1000); // Lot of work with downloading of data for subRegion
                if (rnd.NextDouble() > 0.05)
                    subRegionsWhichWereSuccessfulyDownloaded.Add(subRegion);
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                {
                    Thread.Sleep(500); //Work with removing already downloaded data 
                    return ElevDataManager.DownloadingResult.Canceled;
                }
            }
        }
        if(region.SubRegions.Count == 0)
        {
            Thread.Sleep(1000); // Lot of work with downloading of data for this region
            if(cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return ElevDataManager.DownloadingResult.Canceled;
        }

        foreach (var subRegion in subRegionsWhichWereSuccessfulyDownloaded)
        {
            SetRecursivelySubRegionsToDownloaded(subRegion);
        }
        
        if(region.SubRegions.All(sr => sr.IsDownloaded))
        {
            region.IsDownloaded = true;
            return ElevDataManager.DownloadingResult.Downloaded;
        }
        return ElevDataManager.DownloadingResult.UnableToDownload;
    }

    private void SetRecursivelySubRegionsToDownloaded(IRegion region)
    {
        region.IsDownloaded = true;
        foreach (var subRegion in region.SubRegions)
        {
            SetRecursivelySubRegionsToDownloaded(subRegion);
        }
    }
    
    
    /// <inheritdoc cref="IElevDataDistribution.Initialize"/>
    public void Initialize(){}

    /// <summary>
    /// Demonstrating elevation data type. It returns elevation for every coordinate equal to 3.14.
    /// </summary>
    private class ElevData : IElevData
    {
        /// <inheritdoc cref="IElevData.GetElevation(GeoCoordinates)"/>
        public double? GetElevation(GeoCoordinates coordinates)
        {
            return 3.14;
        }

        /// <inheritdoc cref="IElevData.GetElevation(MapCoordinates,GeoCoordinates)"/>
        public double? GetElevation(MapCoordinates coordinates, GeoCoordinates geoReference, int scale)
        {
            return 3.14;
        }
    }
}
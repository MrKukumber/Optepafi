using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.ElevationDataMan.Regions.Simulating;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;

namespace Optepafi.Models.ElevationDataMan.Distributions.Specific.Simulating;

/// <summary>
/// Elevation data source which simulates work of authorization requiring elevation data distribution.
/// 
/// This type is just demonstrative data source for presenting application functionality.  
/// For more information on elevation data sources which requires authorization for accessing data see <see cref="ICredentialsRequiringElevDataDistribution"/>.  
/// </summary>
public class AuthorizationSimulatingElevDataDistribution : ICredentialsRequiringElevDataDistribution
{
    public static AuthorizationSimulatingElevDataDistribution Instance { get; } = new();
    /// <summary>
    /// Initialize three demonstrating regions. One top region and two subregions.
    /// </summary>
    private AuthorizationSimulatingElevDataDistribution()
    {
        TopRegion notRealRegion = new TopRegion0()
        {
            IsDownloaded = false
        };
        _ = new SubRegion2(notRealRegion)
        {
            IsDownloaded = false
        };
        _ = new SubRegion1(notRealRegion)
        {
            IsDownloaded = true
        };
        AllTopRegions = new HashSet<TopRegion>{ notRealRegion };
    }
    /// <inheritdoc cref="IElevDataDistribution.Name"/>
    public string Name => "Authorization simulating elevation data distribution with name \"Name\" and password \"Password\"";
    /// <inheritdoc cref="IElevDataDistribution.AllTopRegions"/>
    public IReadOnlySet<TopRegion> AllTopRegions { get; }
    /// <inheritdoc cref="IElevDataDistribution.Remove"/>
    public void Remove(Region region)
    {
        Thread.Sleep(500); //Lot of work with removing of regions data
        RemoveRecursivelySubRegions(region);
        SetRecursivelyUpperRegionsToNotDownloaded(region);
    }
    private void RemoveRecursivelySubRegions(Region region)
    {
        region.IsDownloaded = false;
        foreach (var subRegion in region.SubRegions)
        {
            RemoveRecursivelySubRegions(subRegion);
        }
    }
    private void SetRecursivelyUpperRegionsToNotDownloaded(Region region)
    {
        region.IsDownloaded = false;
        if(region is SubRegion subRegion) SetRecursivelyUpperRegionsToNotDownloaded(subRegion.UpperRegion);
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

    /// <inheritdoc cref="ICredentialsRequiringElevDataDistribution.Download"/>
    /// <remarks>
    /// This method simulates downloading of provided region.
    /// At first it checks credentials.
    /// Then it tries to download all subregions with small probability of unsuccessful download.
    /// Responds to cancellation of downloading.
    /// </remarks>
    public ElevDataManager.DownloadingResult Download(Region region, NetworkCredential credential, CancellationToken? cancellationToken)
    {
        Random rnd = new Random();
        if (credential.UserName != "Name" || credential.Password != "Password") return ElevDataManager.DownloadingResult.WrongCredentials;
        List<Region> subRegionsWhichWereSuccessfulyDownloaded = new();
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
    private void SetRecursivelySubRegionsToDownloaded(Region region)
    {
        region.IsDownloaded = true;
        foreach (var subRegion in region.SubRegions)
        {
            SetRecursivelySubRegionsToDownloaded(subRegion);
        }
    }

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
        public double? GetElevation(MapCoordinates coordinates, GeoCoordinates geoReference)
        {
            return 3.14;
        }
    }
}
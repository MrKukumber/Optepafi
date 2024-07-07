using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.ElevationDataMan.Regions.NotReal;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan.Distributions.NotReally;

public class NotReallyElevDataDistribution : ICredentialsRequiringElevDataDistribution
{
    public static NotReallyElevDataDistribution Instance { get; } = new();
    private NotReallyElevDataDistribution()
    {
        TopRegion notRealRegion = new NotRealTopRegion()
        {
            IsDownloaded = false
        };
        new SoNotRealSubRegion(notRealRegion)
        {
            IsDownloaded = false
        };
        new LittleLessNotRealSubReagion(notRealRegion)
        {
            IsDownloaded = true
        };
        AllTopRegions = new HashSet<TopRegion>{ notRealRegion };
    }
    public string Name => "Not really an elevation data distribution";
    public IReadOnlySet<TopRegion> AllTopRegions { get; }
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

    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IGeoLocatedMap map, CancellationToken? cancellationToken)
    {
        Random rnd = new Random();
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return ElevDataManager.ElevDataObtainability.Cancelled;
        if (rnd.NextDouble() > 0.1) return ElevDataManager.ElevDataObtainability.Obtainable;
        return ElevDataManager.ElevDataObtainability.ElevDataNotPresent;
    }

    public IElevData GetElevDataFor(IGeoLocatedMap map, CancellationToken? cancellationToken)
    {
        return new ElevData();
    }

    public ElevDataManager.DownloadingResult Download(Region region, NetworkCredential credential, CancellationToken? cancellationToken)
    {
        Random rnd = new Random();
        if (credential.UserName != "Bobo" || credential.Password != "Boboo") return ElevDataManager.DownloadingResult.WrongCredentials;
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
        if(region.SubRegions.Count == 0) Thread.Sleep(1000); // Lot of work with downloading of data for this region

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

    private class ElevData : IElevData
    {
        public double? GetElevation(GeoCoordinate coordinate)
        {
            return 3.14;
        }

        public double? GetElevation(MapCoordinate coordinate, GeoCoordinate geoReference)
        {
            return 3.14;
        }
    }
}
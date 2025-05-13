using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan.Regions;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Credentials;
using Optepafi.Models.Utils.EsriBilParser;
using Path = System.IO.Path;

namespace Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;

public class UsgsSrtm1ArcSecondGlobal : ICredentialsRequiringElevDataDistribution
{
    public static UsgsSrtm1ArcSecondGlobal Instance { get; } = new ();

    public string Name => "SRTM 1 Arc-Second Global dataset (USGS)";
    public IReadOnlyCollection<ITopRegion> AllTopRegions => _allTopRegions;
    public void Remove(IRegion region)
    {
        if (region is IInnerRegion innerRegion)
            ElevDataRemover.RemoveAndSet(innerRegion);
        else
            Console.WriteLine("Provided region to be removed was not from USGS SRTM 1 arc second global elevation data distribution."); //TODO: log this
    }
    
    public ElevDataManager.DownloadingResult Download(IRegion region, Credentials credentials, CancellationToken? cancellationToken)
    {
        if (region is IInnerRegion innerRegion)
            return ElevDataDownloader.DownloadExtractAndSet(innerRegion, credentials, cancellationToken);
        Console.WriteLine("Provided region to be downloaded was not from USGS SRTM 1 arc second global elevation data distribution."); //TODO: log this
        return ElevDataManager.DownloadingResult.UnableToDownload;
    }

    public ElevDataManager.ElevDataObtainability AreElevDataObtainableFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        ElevDataAssembler assembler = new ElevDataAssembler();
        Task<ElevData> elevData;
        ElevDataManager.ElevDataObtainability result;
        
        switch (map)
        {
            case IBoxBoundedGeoLocMap rectBoundGeoLocMap:
                result = assembler.TryEnsureElevDataForMapInRectangle(rectBoundGeoLocMap.SouthWestBoundingCorner, rectBoundGeoLocMap.NorthEastBoundingCorner, cancellationToken, out elevData);
                break;
            case IBoxBoundedGeoRefMap rectBoundRefLocMap:
                result = assembler.TryEnsureElevDataForMapInRectangle(rectBoundRefLocMap.BottomLeftBoundingCorner.ToGeoCoordinates(rectBoundRefLocMap.RepresentativeLocation, rectBoundRefLocMap.Scale), rectBoundRefLocMap.TopRightBoundingCorner.ToGeoCoordinates(rectBoundRefLocMap.RepresentativeLocation, rectBoundRefLocMap.Scale), cancellationToken, out elevData);
                break;
            // case IByReferenceAreaIntersectionQueryableMap referenceAreaMap:
                // break;
            // case IDirectlyAreaIntersectionQueryableMap directAreaMap:
                // break;
            default:
                return ElevDataManager.ElevDataObtainability.NotSupportedMap;
        }
        if (result is ElevDataManager.ElevDataObtainability.Obtainable)
            _requestedElevData[map] = elevData;
        return result;
    }


    public IElevData GetElevDataFor(IAreaQueryableMap map, CancellationToken? cancellationToken)
    {
        if (_requestedElevData.Remove(map, out var elevData))
            return elevData.Result;
        throw new InvalidOperationException( "Obtainability of elevation data for provided map was not checked. The were therefore not prepared for aquisition.");
    }

    public void Initialize()
    {
        if(!Directory.Exists(_elevDataFilesDir))
            Directory.CreateDirectory(_elevDataFilesDir);
        foreach (var scene in _scenes.Values)
            scene.Initialize();
        foreach (var topRegion in _allTopRegions)
            topRegion.InitializeRecursively();
    }

    public CredentialsType CredType { get; } = CredentialsType.UserNameAndAuthenticationToken;
    

    private HashSet<IInnerTopRegion> _allTopRegions;
    private string _elevDataFilesDir; 
    
    private Dictionary<(int, int), Scene> _scenes;
    private Dictionary<(int, int), int> _scenesUsefulnessReferenceCount;
    private Dictionary<IAreaQueryableMap, Task<ElevData>> _requestedElevData;
    
    private UsgsSrtm1ArcSecondGlobal()
    {
        _allTopRegions = [SlovakiaRegion.Instance];
        _elevDataFilesDir = Path.Combine("elevData", "USGS", "SRTM_1ArcSecond_Global"); 

        HashSet<(int, int)> usedScenesPositions = [];
        foreach (var topRegion in _allTopRegions)
            foreach (var scenePosition in topRegion.SortedUsedScenesPositions)
                usedScenesPositions.Add(scenePosition);
        
        _scenes = usedScenesPositions.ToDictionary(scenePos => scenePos, scenePos => new Scene(scenePos));
        _scenesUsefulnessReferenceCount = usedScenesPositions.ToDictionary(scenePos => scenePos, _ => 0);
        _requestedElevData = new();
    }
    
    
    
    
    
    
    
    
    
    private interface IInnerRegion : IRegion
    {
        // it is important the collection of scene indices is sorted to prevent deadlock when scenes of regions are locked for their downloading/removing.
        SortedSet<(int, int)> SortedUsedScenesPositions { get; }
        
        IReadOnlyCollection<IInnerSubRegion> InnerSubRegions { get; }
        public bool InitializeRecursively()
        {
            bool isSomeSubRegionNotDownloaded = false;
            foreach (var region in SubRegions)
                if(region is IInnerRegion innerRegion)
                    isSomeSubRegionNotDownloaded = isSomeSubRegionNotDownloaded || innerRegion.InitializeRecursively();
            
            if (isSomeSubRegionNotDownloaded) return false;
            
            foreach (var scenePosition in SortedUsedScenesPositions)
                if (!Instance._scenes[scenePosition].Downloaded)
                    return false;

            foreach (var scenePosition in SortedUsedScenesPositions)
            {
                Instance._scenesUsefulnessReferenceCount[scenePosition]++;
            }
            
            IsDownloaded = true;
            return true;
        }
    }
    private interface IInnerTopRegion : IInnerRegion, ITopRegion;

    private interface IInnerSubRegion : IInnerRegion, ISubRegion
    {
        IInnerRegion InnerUpperRegion { get; }
    }

    private class SlovakiaRegion : 
        Regions.World.Europe.Slovakia.SlovakiaRegion,
        IInnerTopRegion
    {
        public static SlovakiaRegion Instance { get; } = new();
        private SlovakiaRegion() { }
        
        public IReadOnlyCollection<IInnerSubRegion> InnerSubRegions => new HashSet<IInnerSubRegion>(); 
        public override IReadOnlyCollection<ISubRegion> SubRegions => InnerSubRegions;
        public SortedSet<(int, int)> SortedUsedScenesPositions { get; } = [(47,17), (47,18), (48,16), (48,17), (48, 18), (48,19), (48,20), (48,21), (48,22), (49,17), (49,18), (49,19), (49,20), (49,21), (49,22)];
    }

    private class Scene
    {
        private (int longitude, int latitude) _position;
        private string _directoryName;
        private string _bilFileName;
        public Scene((int latitude, int longitude) geoDegreeCoords)
        {
            _position = geoDegreeCoords;
            _directoryName = (geoDegreeCoords.latitude >= 0 ? "n" : "s") + Math.Abs(geoDegreeCoords.latitude).ToString("D2") + "_" + (geoDegreeCoords.longitude >= 0 ? "e" : "w") + Math.Abs(geoDegreeCoords.longitude).ToString("D3") + "_1arc_v3_bil" ;
            _bilFileName = (geoDegreeCoords.latitude >= 0 ? "n" : "s") + Math.Abs(geoDegreeCoords.latitude).ToString("D2") + "_" + (geoDegreeCoords.longitude >= 0 ? "e" : "w") + Math.Abs(geoDegreeCoords.longitude).ToString("D3") + "_1arc_v3.bil" ;
            Downloaded = false;
        }

        public (int lat, int lon) Position => _position;
        public string DirectoryName => _directoryName;

        public void Initialize()
        {
            if (Directory.Exists(Path.Combine(Instance._elevDataFilesDir, _directoryName)))
                Downloaded = true;
            else
                Downloaded = false;
        }
        
        public bool Downloaded { get; set; }

        public int[,] AcquireData()
        {
            var parser = new EsriBilParser();
            try {
                parser.ReadBilHeader(Path.Combine(Instance._elevDataFilesDir, _directoryName, _bilFileName));
            } catch (DirectoryNotFoundException e) {
                Console.WriteLine("Directory with bil data file corresponding to coordinates " + _position + " was not found."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (FileNotFoundException e) {
                Console.WriteLine("Header file of bil data corresponding to coordinates " + _position + " was not found."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (IOException e) {
                Console.WriteLine("Header file of bil data corresponding to coordinates" + _position + "  could not be read."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (FormatException e) {
                Console.WriteLine(e.Message); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            }

            try {
                int[][,] allBands = parser.GetAllBandsInts();
                return allBands[0];
            }
            catch (DirectoryNotFoundException e) {
                Console.WriteLine("Directory with bil data file corresponding to coordinates " + _position + " was not found."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (FileNotFoundException e) {
                Console.WriteLine("Bil data file corresponding to coordinates " + _position + " was not found."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (EndOfStreamException e) {
                Console.WriteLine("Bil data file corresponding to coordinates " + _position + " was of invalid format not corresponding to size defined in header."); //TODO: log this exception
                return new int[2, 2] { { 0, 0 }, { 0, 0 } };
            } catch (IOException e) {
                Console.WriteLine("Bil data file corresponding to coordinates" + _position + " could not be read."); //TODO: log this exception
                return new int[2, 2] { { 0, 0}, { 0, 0 } }; 
            }
        }
    }

    private class ElevData(Dictionary<(int, int), int[,]> relevantElevData) : IElevData
    {
        public double? GetElevation(MapCoordinates coordinates, GeoCoordinates geoReference, int scale)
            => GetElevation(coordinates.ToGeoCoordinates(geoReference, scale));
        
        public double? GetElevation(GeoCoordinates coordinates)
        {
            int latitudeDegree = (int)coordinates.Latitude;
            int longitudeDegree = (int)coordinates.Longitude;
            if (relevantElevData.ContainsKey((latitudeDegree, longitudeDegree)))
            {
                double row = (1-(coordinates.Latitude - latitudeDegree)) * (relevantElevData[(latitudeDegree, longitudeDegree)].GetLength(0)-1);
                double coll = (coordinates.Longitude - longitudeDegree) * (relevantElevData[(latitudeDegree, longitudeDegree)].GetLength(1)-1);
                return BilinearInterpolation(relevantElevData[(latitudeDegree, longitudeDegree)], row, coll);
            }
            return null;
        }

        private double BilinearInterpolation(int[,] matrix, double r, double c)
        {
            int r1 = (int)r;
            int c1 = (int)c;
            int r2 = r1 + 1;
            int c2 = c1 + 1;
            
            double e_r_c1 = (r2 - r) * matrix[r1, c1] + (r - r1) * matrix[r2, c1]; 
            double e_r_c2 = (r2 - r) * matrix[r1, c2] + (r - r1) * matrix[r2, c2]; 
            
            double e_r_c = (c2 - c) * e_r_c1 + (c - c1) * e_r_c2;
            return e_r_c;
        }
    }


    
    
    
    private class ElevDataAssembler
    {
        private ElevDataManager.ElevDataObtainability _obtainabilitOfAllScenesElevData;
        private AutoResetEvent _allSceneseAvailabityCheckEvent;
        
        public ElevDataAssembler()
        {
            _obtainabilitOfAllScenesElevData = ElevDataManager.ElevDataObtainability.Obtainable;
            _allSceneseAvailabityCheckEvent = new AutoResetEvent(false);
        }
        
        public ElevDataManager.ElevDataObtainability TryEnsureElevDataForMapInRectangle(GeoCoordinates southWestCorner, GeoCoordinates northEastCorner, CancellationToken? cancellationToken, out Task<ElevData> requestedElevData)
        {
            (int, int) swScenePosition = ((int)southWestCorner.Latitude,(int)southWestCorner.Longitude);
            (int, int) neScenePosition = ((int)northEastCorner.Latitude,(int)northEastCorner.Longitude);
            requestedElevData = Task.Run(() =>
            {
                 Dictionary<(int, int), int[,]> data = new Dictionary<(int, int), int[,]>(); 
                 RecursivelyCollectElevData(swScenePosition, neScenePosition, data, cancellationToken);
                 return new ElevData(data);
            });
            _allSceneseAvailabityCheckEvent.WaitOne();
            return _obtainabilitOfAllScenesElevData;
        }

        private void RecursivelyCollectElevData((int, int) swScenePosition, (int, int) neScenePosition, Dictionary<(int, int), int[,]> data, CancellationToken? cancellationToken, int i = -1, int j = 0)
        {
            if (++i > neScenePosition.Item1 - swScenePosition.Item1) { i = 0; ++j; }

            Scene scene;
            if (Instance._scenes.ContainsKey((swScenePosition.Item1 + i, swScenePosition.Item2 + j)))
                scene = Instance._scenes[(swScenePosition.Item1 + i, swScenePosition.Item2 + j)];
            else
            {
                _obtainabilitOfAllScenesElevData = ElevDataManager.ElevDataObtainability.ElevDataNotPresent;
                while (!_allSceneseAvailabityCheckEvent.Set()) Thread.Sleep(10);
                return;
            }

            lock (scene)
            {
                if (!scene.Downloaded)
                {
                    _obtainabilitOfAllScenesElevData = ElevDataManager.ElevDataObtainability.ElevDataNotPresent;
                    while (!_allSceneseAvailabityCheckEvent.Set()) Thread.Sleep(10);
                    return;
                }

                if (i + swScenePosition.Item1 == neScenePosition.Item1 && j + swScenePosition.Item2 == neScenePosition.Item2)
                    while (!_allSceneseAvailabityCheckEvent.Set()) Thread.Sleep(10);
                else
                    RecursivelyCollectElevData(swScenePosition, neScenePosition, data, cancellationToken, i, j);

                if (_obtainabilitOfAllScenesElevData is not ElevDataManager.ElevDataObtainability.Obtainable) return;
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                {
                    _obtainabilitOfAllScenesElevData = ElevDataManager.ElevDataObtainability.Cancelled;
                    return;
                }
                data[(swScenePosition.Item1 + i, swScenePosition.Item2 + j)] = scene.AcquireData();
            }
        }
    }
    
    
    private static class ElevDataDownloader
    {
        public static ElevDataManager.DownloadingResult DownloadExtractAndSet(IInnerRegion region, Credentials credentials, CancellationToken? cancellationToken)
        {
            return  RecursivelyLockAllScenesAndProceed(region, credentials, cancellationToken);
        }

        private static ElevDataManager.DownloadingResult RecursivelyLockAllScenesAndProceed(IInnerRegion region, Credentials credentials, CancellationToken? cancellationToken, int i = 0)
        {
            if (i == region.SortedUsedScenesPositions.Count)
                return Proceed(region, credentials, cancellationToken);
            lock (Instance._scenes[region.SortedUsedScenesPositions.ElementAt(i)])
                return RecursivelyLockAllScenesAndProceed(region, credentials, cancellationToken, i + 1);
        }

        private static ElevDataManager.DownloadingResult Proceed(IInnerRegion region, Credentials credentials, CancellationToken? cancellationToken, int i = 0)
        {
            Scene[] scenes = region.SortedUsedScenesPositions
                .Where(scenePosition => !Instance._scenes[scenePosition].Downloaded)
                .Select(scenePosition => Instance._scenes[scenePosition])
                .ToArray();
            var result = DownloadElevData(scenes, credentials, cancellationToken);
            if (result is ElevDataManager.DownloadingResult.Downloaded)
            {
                foreach (Scene scene in scenes) scene.Downloaded = true;
                SetRecursivelySubregionsToDownloadedAndAdjustScenesUsefulnessReferenceCount(region);
            }
            return result;
        }
        
        private static void SetRecursivelySubregionsToDownloadedAndAdjustScenesUsefulnessReferenceCount(IInnerRegion region)
        {
            if (!region.IsDownloaded)
            {
                region.IsDownloaded = true;
                foreach ((int, int) scenePosition in region.SortedUsedScenesPositions)
                    Instance._scenesUsefulnessReferenceCount[scenePosition] += 1;
            }
            foreach (IInnerSubRegion subRegion in region.InnerSubRegions)
                SetRecursivelySubregionsToDownloadedAndAdjustScenesUsefulnessReferenceCount(subRegion);
        }
        private static ElevDataManager.DownloadingResult DownloadElevData(Scene[] scenes, Credentials credentials, CancellationToken? cancellationToken)
        {
            var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(scenes.Select(scene => scene.Position).ToArray(), Instance._elevDataFilesDir, credentials, cancellationToken, out List<string>? downloadedFilesDirs);
            if (result is not UsgsM2MApiHttpClient.DownloadingResult.Downloaded)
                return result switch
                {
                    UsgsM2MApiHttpClient.DownloadingResult.Canceled => ElevDataManager.DownloadingResult.Canceled,
                    UsgsM2MApiHttpClient.DownloadingResult.WrongCredentials => ElevDataManager.DownloadingResult.WrongCredentials, 
                    _ => ElevDataManager.DownloadingResult.UnableToDownload
                };
            
            var downloadingResult = ExtractElevData(downloadedFilesDirs, cancellationToken);
            return downloadingResult;
        }

        private static ElevDataManager.DownloadingResult ExtractElevData(List<string> downloadedFilesDirs, CancellationToken? cancellationToken)
        {
            ElevDataManager.DownloadingResult downloadingResult = ElevDataManager.DownloadingResult.Downloaded;
            
            List<string> extractedDirs = new ();
            foreach (string downloadedFileDir in downloadedFilesDirs)
            {
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                { downloadingResult = ElevDataManager.DownloadingResult.Canceled; break; }
                
                string extractPath =  downloadedFileDir.Substring(0, downloadedFileDir.Length - 4);
                try {
                    System.IO.Compression.ZipFile.ExtractToDirectory(downloadedFileDir, extractPath, true);
                    extractedDirs.Add(extractPath);
                } catch (IOException) 
                { downloadingResult = ElevDataManager.DownloadingResult.UnableToDownload; break; }
            }
            
            if (downloadingResult is not ElevDataManager.DownloadingResult.Downloaded)
                try {
                    foreach (var extractedDir in extractedDirs)
                        Directory.Delete(extractedDir, true);
                } catch (IOException) { }
                
            
            foreach (string downloadedFileDir in downloadedFilesDirs)
                try { File.Delete(downloadedFileDir); }
                catch (IOException) {}

            return downloadingResult;
        }
    }

    private static class ElevDataRemover
    {
        public static void RemoveAndSet(IInnerRegion region)
        {
            RecursivelyLockAllScenesAndProceed(region);
        }

        private static void RecursivelyLockAllScenesAndProceed(IInnerRegion region, int i = 0)
        {
            if (i == region.SortedUsedScenesPositions.Count)
                Proceed(region);
            else
                lock (Instance._scenes[region.SortedUsedScenesPositions.ElementAt(i)])
                    RecursivelyLockAllScenesAndProceed(region, i + 1);
        }

        private static void Proceed(IInnerRegion region)
        {
            Scene[] scenesToBeRemoved = region.SortedUsedScenesPositions.Where(scenePosition => Instance._scenesUsefulnessReferenceCount[scenePosition] == 1).Select(scenePosition => Instance._scenes[scenePosition]).ToArray();
            RemoveElevData(scenesToBeRemoved);
            foreach (Scene scene in scenesToBeRemoved) scene.Downloaded = false;
            SetRecursivelySubregionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(region);
            if(region is IInnerSubRegion subRegion) SetRecursivelyUpperRegionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(subRegion.InnerUpperRegion);
        }

        private static void SetRecursivelySubregionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(IInnerRegion region)
        {
            region.IsDownloaded = false;
            foreach ((int, int) scenePosition in region.SortedUsedScenesPositions)
                Instance._scenesUsefulnessReferenceCount[scenePosition] -= 1;
            foreach (var subRegion in region.InnerSubRegions)
                SetRecursivelySubregionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(subRegion);
        }

        private static void SetRecursivelyUpperRegionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(IInnerRegion region)
        {
            if (region.IsDownloaded)
            {
                region.IsDownloaded = false;
                foreach ((int, int) scenePosition in region.SortedUsedScenesPositions)
                    Instance._scenesUsefulnessReferenceCount[scenePosition] -= 1;
            }
            if (region is IInnerSubRegion subRegion) SetRecursivelyUpperRegionsToNotDownloadedAndAdjustScenesUsefulnessReferenceCount(subRegion.InnerUpperRegion);
        }

        private static void RemoveElevData(Scene[] scenes)
        {
            foreach (var scene in scenes)
            {
                try { Directory.Delete(Path.Combine(Instance._elevDataFilesDir, scene.DirectoryName), true); }
                catch (IOException e) { }
            }
        }
    }
}
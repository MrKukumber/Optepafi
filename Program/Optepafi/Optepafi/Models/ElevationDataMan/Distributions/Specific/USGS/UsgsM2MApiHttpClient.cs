using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Optepafi.Models.Utils.Credentials;
using Path = System.IO.Path;

namespace Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;

//TODO: comment
public class UsgsM2MApiHttpClient
{
    public enum DownloadingResult { Downloaded, Canceled, WrongCredentials, UnableConnectToInternet, NoResultForDownloadingFound, UnableToWriteDataToFile, HttpRequestErrorCodeReceived  }
    public static UsgsM2MApiHttpClient Instance { get; set; } = new();
    
    private Semaphore _semaphore;
    private string _serviceUrl ;

    private UsgsM2MApiHttpClient()
    {
        int maxThreads = Environment.ProcessorCount - 1;
        _semaphore = new Semaphore(maxThreads, maxThreads);
        _serviceUrl = "https://m2m.cr.usgs.gov/api/api/json/stable/";
    }

    
    // Parallel calls of this method must ensure, that they download different scenes, if the do, it is so they are not downloaded to same directory
    public DownloadingResult RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes( (int, int)[] positionsOfScenesToBeDownloaded, string outputDirPath, Credentials credentials, CancellationToken? cancellationToken, out List<string>? downloadedFilesDirs)
    {
        downloadedFilesDirs = null;
        if (positionsOfScenesToBeDownloaded.Length == 0) return DownloadingResult.Downloaded;

        List<Task<(DownloadingResult, string)>> threads = [];
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Clear();

        Console.WriteLine("Searching datasets..." + Environment.NewLine); //TODO: log
        
        if (!TryLogin(client, credentials, cancellationToken, out string? apiKey, out var downloadingResultIfProblem))
            return downloadingResultIfProblem.Value;
        
        Console.WriteLine("API Key: " + apiKey + Environment.NewLine); //TODO: log

        client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);
        
        string datasetName = "srtm_v3";
        
        (int lat, int lon) lowerLeft = positionsOfScenesToBeDownloaded.First();
        (int lat, int lon) upperRight = positionsOfScenesToBeDownloaded.First();
        foreach ((int lat, int lon) scenePosition in positionsOfScenesToBeDownloaded)
        {
            if (scenePosition.lat < lowerLeft.lat) lowerLeft.lat = scenePosition.lat;
            if (scenePosition.lon < lowerLeft.lon) lowerLeft.lon = scenePosition.lon;
            if (scenePosition.lat > upperRight.lat) upperRight.lat = scenePosition.lat;
            if (scenePosition.lon > upperRight.lon) upperRight.lon = scenePosition.lon;
        }

        SpatialFilterMbr spatialFilter = new SpatialFilterMbr
        {
            LowerLeft = new Coordinate { Latitude = lowerLeft.lat + 0.5, Longitude = lowerLeft.lon + 0.5 },
            UpperRight = new Coordinate { Latitude = upperRight.lat + 0.5, Longitude = upperRight.lon + 0.5 }
        };

        if (!TrySearchDatasets(client, positionsOfScenesToBeDownloaded, datasetName, spatialFilter, cancellationToken, out var datasets, out downloadingResultIfProblem))
        { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value; }

        Console.WriteLine("Found " + datasets.Length + " datasets" + Environment.NewLine); //TODO: log
        
        // download datasets
        foreach (Dataset dataset in datasets)
        {
            // Because I've ran this before I know that I want srtm_v3, I don't want to download anything I don't want 
            // so we will skip any other datasets that might be found, logging it in case I wnat to look into downloading
            // that data in the future
            if (dataset.DatasetAlias != datasetName)
            {
                Console.WriteLine("Found dataset " + dataset.CollectionName + " but skipping it." + Environment.NewLine); //TODO: log
                continue;
            }
            
            Console.WriteLine("Searching scenes..." + Environment.NewLine + Environment.NewLine); //TODO: log
            if (!TrySearchScenes(client, dataset, datasetName, spatialFilter, cancellationToken, out SceneRecords? scenes, out bool continueWithoutDatset, out downloadingResultIfProblem))
            { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value;}


            // Did we find anything?
            if (scenes.RecordsReturned > 0)
            {
                if (!TryCollectAvailableDownloads(client, positionsOfScenesToBeDownloaded, scenes, dataset, cancellationToken, out List<Download>? downloads, out downloadingResultIfProblem))
                { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value; }

                // Did we find products?
                if (downloads.Count > 0)
                {
                    
                    string label = DateTime.Now.ToString("%Y%m%d_%H%M%S");
                    
                    if (!TryRequestDownloads(client, downloads, label, cancellationToken, out RequestResults? requestResults, out downloadingResultIfProblem))
                    { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value;}
                    
                    // PreparingDownloads has a valid link that can be used but data may not be immediately available
                    // Call the download-retrieve method to get download that is available for immediate download
                    if (requestResults.PreparingDownloads.Length > 0)
                    {

                        if (!TryRetrieveDownloads(client, label, cancellationToken, out MoreDownloadUrls? moreDownloadUrls, out downloadingResultIfProblem))
                        { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value;}
                        
                        List<int> downloadIds = new();
                        
                        foreach(var download in moreDownloadUrls.Available)
                            if (requestResults.NewRecords.ContainsKey(download.DownloadId.ToString()) || requestResults.DuplicateProducts.ContainsKey(download.DownloadId.ToString()))
                            {
                                downloadIds.Add(download.DownloadId);
                                threads.Add(DownloadFileAsync(client, download.Url, outputDirPath, cancellationToken));
                            }

                        foreach (var download in moreDownloadUrls.Requested)
                            if (requestResults.NewRecords.ContainsKey(download.DownloadId.ToString()) || requestResults.DuplicateProducts.ContainsKey(download.DownloadId.ToString()))
                            {
                                downloadIds.Add(download.DownloadId);
                                threads.Add(DownloadFileAsync(client, download.Url, outputDirPath, cancellationToken));
                            }
                        
                        // Didn't get all of the requested downloads, call the download-retrieve method again probably after 30 seconds
                        while (downloadIds.Count < downloads.Count - requestResults.Failed.Count)
                        {
                            int preparingDownloads = downloads.Count - downloadIds.Count - requestResults.Failed.Count;
                            Console.WriteLine(Environment.NewLine + preparingDownloads + " downloads are not available. Waiting for 30 seconds." + Environment.NewLine); //TODO: log
                            if (cancellationToken is not null && cancellationToken.Value.WaitHandle.WaitOne(30000))
                            {
                                CollectDownloadingThreadsAndDeleteDownloadedFiles(threads);
                                TryLogOut(client, cancellationToken); return DownloadingResult.Canceled;
                            }
                            else
                                Thread.Sleep(30000);

                            Console.WriteLine("Trying to retrieve data" + Environment.NewLine); //TODO: log

                            if (!TryRetrieveDownloads(client, label, cancellationToken, out moreDownloadUrls, out downloadingResultIfProblem))
                            {
                                CollectDownloadingThreadsAndDeleteDownloadedFiles(threads);
                                TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value;
                            }

                            foreach(var download in moreDownloadUrls.Available)
                                if (!downloadIds.Contains(download.DownloadId) && (requestResults.NewRecords.ContainsKey(download.DownloadId.ToString()) || requestResults.DuplicateProducts.ContainsKey(download.DownloadId.ToString())))
                                {
                                    downloadIds.Add(download.DownloadId);
                                    threads.Add(DownloadFileAsync(client, download.Url, outputDirPath, cancellationToken));
                                }
                        }
                    }
                    else
                    {
                        // Get all available downloads
                        foreach (var download in requestResults.AvailableDownloads)
                            threads.Add(DownloadFileAsync(client, download.Url, outputDirPath, cancellationToken));
                    }
                }
            }
            else
            {
                Console.WriteLine("Search found no results" + Environment.NewLine); //TODO: log
                TryLogOut(client, cancellationToken);
                return DownloadingResult.NoResultForDownloadingFound;
            }
        }
        
        Console.WriteLine("Downloading files... Please do not close the program" + Environment.NewLine); //TODO: log

        if (!TryCollectDownloadingThreads(threads, out downloadedFilesDirs, out downloadingResultIfProblem))
        { TryLogOut(client, cancellationToken); return downloadingResultIfProblem.Value; }
        
        Console.WriteLine("Complete Downloading"); //TODO: log

        if(TryLogOut(client, cancellationToken)) Console.WriteLine("Logged Out" + Environment.NewLine); //TODO: log
        else Console.WriteLine("Logout Failed" + Environment.NewLine); //TODO: log
        
        return DownloadingResult.Downloaded;
    }
    
    private bool TryLogin(HttpClient client, Credentials credentials, CancellationToken? cancellationToken, out string? apiKey, out DownloadingResult? downloadingResultIfProblem)
    {
        downloadingResultIfProblem = null;
        apiKey = null;
        
        string username = credentials.UserName;
        string token = credentials.AuthenticationToken;
        
        AuthenticationPayload ap = new AuthenticationPayload { Username = username, Token = token };
        AuthenticationResponseContent? arc;

        Console.WriteLine(Environment.NewLine + "Running Scripts..." + Environment.NewLine); //TODO: log
        HttpStatusCode statusCode;
        
        try { statusCode = SendRequest(client, "login-token", ap, cancellationToken, out arc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, arc.ErrorCode, cancellationToken, out DownloadingResult? resultIfProblem)) { downloadingResultIfProblem = resultIfProblem.Value; return false; }
        apiKey = arc.Data;

        return true;
    }
    private bool TrySearchDatasets(HttpClient client, (int, int)[] positionsOfScenesToBeDownloaded, string datasetName, SpatialFilterMbr spatialFilter, CancellationToken? cancellationToken, out Dataset[]? datasets, out DownloadingResult? downloadingResultIfProblem)
    {
        downloadingResultIfProblem = null;
        datasets = null;

        DatasetSearchPayload dsp = new DatasetSearchPayload
        {
            DatasetName = datasetName,
            SpatialFilter = spatialFilter
        };
        DatasetSearchResponseContent? dsrc;
        
        HttpStatusCode statusCode;

        try { statusCode = SendRequest(client, "dataset-search", dsp, cancellationToken, out dsrc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false ; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, dsrc.ErrorCode, cancellationToken, out var resultIfProblem)) { downloadingResultIfProblem = resultIfProblem.Value; return false; }
        datasets = dsrc.Data;
        
        
        return true;
    }
    private bool TrySearchScenes(HttpClient client, Dataset dataset, string datasetName, SpatialFilterMbr spatialFilter, CancellationToken? cancellationToken, out SceneRecords? scenes, out bool continueWithoutDatset, out DownloadingResult? downloadingResultIfProblem)
    {
        scenes = null;
        continueWithoutDatset = false;
        downloadingResultIfProblem = null;
        

        var ssp = new SceneSearchPayload
        {
            DatasetName = dataset.DatasetAlias,
            SceneFilter = new SceneFilter
            {
                SpatialFilter = spatialFilter
            }
        };
        SceneSearchResponseContent? ssrc;

        // Now I need to run a scene search to find data to download
        HttpStatusCode statusCode;

        try{ statusCode = SendRequest(client, "scene-search", ssp, cancellationToken, out ssrc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, ssrc.ErrorCode, cancellationToken, out var resultIfProblem)) { downloadingResultIfProblem = resultIfProblem.Value; return false; }
        scenes = ssrc.Data;
        return true;
    }
    private bool TryCollectAvailableDownloads(HttpClient client, (int, int)[] positionsOfScenesToBeDownloaded, SceneRecords scenes, Dataset dataset, CancellationToken? cancellationToken, out List<Download>? downloads, out DownloadingResult? downloadingResultIfProblem)
    {
        downloads = null;
        downloadingResultIfProblem = null;
        
        // Aggregate a list of scene ids
        List<string> sceneIds = new();
        foreach (var result in scenes.Results)
            // Add scene to the list I would like to download only if it corresponds to some requested scene position 
            if (positionsOfScenesToBeDownloaded.Contains((result.SpatialBounds.Coordinates[0][0][1], result.SpatialBounds.Coordinates[0][0][0])))
                sceneIds.Add(result.EntityId);

        DownloadOptionsPayload dop = new DownloadOptionsPayload
        {
            DatasetName = dataset.DatasetAlias,
            EntityIds = sceneIds
        };
        DownloadOptionsResponseContent? dorc; 
        
        HttpStatusCode statusCode;
        
        // Find the download options fo these scenes
        // NOTE :: Remeber the scene list cannot exceed 50,000 items!
        try { statusCode = SendRequest(client, "download-options", dop, cancellationToken, out dorc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, dorc.ErrorCode, cancellationToken, out var resultIfProblem)) {downloadingResultIfProblem = resultIfProblem.Value; return false; }

        // Aggregate a list of available products of Bil file format
        downloads = [];
        foreach (var product in dorc.Data)
            // Make sure the product is available for this scene
            if (product.Available)
                // Filtering format of downloaded data
                if (product.ProductName == "BIL 1 Arc-second")
                    downloads.Add(new Download{EntityId = product.EntityId, ProductId = product.Id});
        
        return true;
    }
    private bool TryRequestDownloads(HttpClient client, List<Download> downloads, string label, CancellationToken? cancellationToken, out RequestResults? requestResults, out DownloadingResult? downloadingResultIfProblem)
    {
        requestResults = null;
        downloadingResultIfProblem = null;
        
        // set a label for the download request
        DownloadRequestPayload drp = new DownloadRequestPayload { Downloads = downloads, Label = label };
        DownloadRequestResponseContent? drrc; 
        
        HttpStatusCode statusCode;
        
        // Call the download to get the direct download urls
        try { statusCode = SendRequest(client, "download-request", drp, cancellationToken, out drrc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, drrc.ErrorCode, cancellationToken, out var resultIfProblem)) {downloadingResultIfProblem = resultIfProblem.Value; return false; }
        requestResults = drrc.Data;
        
        return true;
    }
    private bool TryRetrieveDownloads(HttpClient client, string label, CancellationToken? cancellationToken, out MoreDownloadUrls? moreDownloadUrls, out DownloadingResult? downloadingResultIfProblem)
    {
        moreDownloadUrls = null;
        downloadingResultIfProblem = null;
        
        DownloadRetrievePayload drep = new DownloadRetrievePayload { Label = label };
        DownloadRetrieveResponseContent? drerc;
        
        HttpStatusCode statusCode;

        try { statusCode = SendRequest(client, "download-retrieve", drep, cancellationToken, out drerc); }
        catch (OperationCanceledException) { downloadingResultIfProblem = DownloadingResult.Canceled; return false; }
        catch (HttpRequestException) { downloadingResultIfProblem = DownloadingResult.UnableConnectToInternet; return false; }
        if (IsThereSomeProblem(statusCode, drerc.ErrorCode, cancellationToken, out var resultIfProblem)) {downloadingResultIfProblem = resultIfProblem.Value; return false; }
        moreDownloadUrls = drerc.Data;
        
        return true;
    }
    private bool TryCollectDownloadingThreads(List<Task<(DownloadingResult, string)>> threads, out List<string>? downloadedFilesDirs, out DownloadingResult? downloadingResultIfProblem)
    {
        downloadingResultIfProblem = null;
        downloadedFilesDirs = null;
        
        List<(DownloadingResult result, string fileDir)> successfulyDownloadedFiles = [];
        foreach (var thread in threads)
        {
            (DownloadingResult result, string fileDir) output = thread.Result;
            if (output.result is DownloadingResult.Downloaded)
                successfulyDownloadedFiles.Add((output.result, output.fileDir));
            else
                if (downloadingResultIfProblem is not DownloadingResult.Canceled)
                    downloadingResultIfProblem = output.result;
        }

        if (downloadingResultIfProblem is not null)
        {
            foreach (var successfulyDownloadedFile in successfulyDownloadedFiles)
                File.Delete(successfulyDownloadedFile.fileDir);
            return false;
        }
        
        downloadedFilesDirs = successfulyDownloadedFiles.Select(x => x.fileDir).ToList();
        return true;
    }

    private void CollectDownloadingThreadsAndDeleteDownloadedFiles(List<Task<(DownloadingResult, string)>> threads)
    {
         List<(DownloadingResult result, string fileDir)> successfulyDownloadedFiles = [];
         foreach (var thread in threads)
         {
             (DownloadingResult result, string fileDir) output = thread.Result;
             if (output.result is DownloadingResult.Downloaded)
                 successfulyDownloadedFiles.Add((output.result, output.fileDir));
         }
         foreach (var successfulyDownloadedFile in successfulyDownloadedFiles)
             File.Delete(successfulyDownloadedFile.fileDir);
    }
    private bool TryLogOut(HttpClient client, CancellationToken? cancellationToken)
    {
        LogoutResponseContent? lrc;
        
        try { _ = SendRequest<LogoutPayload, LogoutResponseContent>(client, "logout", null, cancellationToken, out lrc); }
        catch (OperationCanceledException e) { return false; }
        catch (HttpRequestException) { return false; }
        if (lrc.Data is not null) return false; 
        
        return true; 
    }
    
    private HttpStatusCode SendRequest<TPayload, TResponseContent>(HttpClient client, string endpoint, TPayload? payload, CancellationToken? cancellationToken, out TResponseContent? responseContent)
    {
        string url = _serviceUrl + endpoint;
        HttpResponseMessage response;

        try
        {
            if (cancellationToken is CancellationToken ct)
                response = client.PostAsJsonAsync(url, payload, ct).Result;
            else
                response = client.PostAsJsonAsync(url, payload).Result;
        }
        catch (AggregateException e)
        {
            foreach (var innerException in e.InnerExceptions)
                if (innerException is (OperationCanceledException or HttpRequestException)) throw innerException;
            throw;
        }

        string jsonContent = response.Content.ReadAsStringAsync().Result;
        if (endpoint == "download-request") jsonContent = ReplaceEmptyArraysWithNulls(jsonContent);
        responseContent = JsonSerializer.Deserialize<TResponseContent>(jsonContent);
        return response.StatusCode;
    }

    private string ReplaceEmptyArraysWithNulls(string jsonContent) => jsonContent.Replace("[]", "null");

    private async Task<(DownloadingResult, string)> DownloadFileAsync(HttpClient client, string url, string outputDirPath, CancellationToken? cancellationToken)
    {
        _semaphore.WaitOne();
        HttpResponseMessage response;
        try
        {
            if (cancellationToken is CancellationToken ct)
                response = await client.GetAsync(url, ct);
            else
                response = await client.GetAsync(url);
            HttpContent responseContent = response.Content;
            string fileName = responseContent.Headers.ContentDisposition.FileName.Substring(1, responseContent.Headers.ContentDisposition.FileName.Length - 2);
            Console.WriteLine($"Downloading {fileName} ..." + Environment.NewLine);
            string fileDir = Path.Combine(outputDirPath, fileName);
            try {
                using (FileStream fs = new(fileDir, new FileStreamOptions { Mode = FileMode.OpenOrCreate, Access = FileAccess.Write}))
                { responseContent.ReadAsStream().CopyTo(fs); }
            } catch (IOException e) { return (DownloadingResult.UnableToWriteDataToFile, ""); } 

            Console.WriteLine($"Downloaded {fileName}" + Environment.NewLine);
            _semaphore.Release();
            return (DownloadingResult.Downloaded, fileDir);
        } catch (OperationCanceledException e) { return (DownloadingResult.Canceled, "");  }
    }


    private bool IsThereSomeProblem(HttpStatusCode statusCode, string? errorCode, CancellationToken? cancellationToken, out DownloadingResult? resultIfProblem)
    {
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
        { resultIfProblem = DownloadingResult.Canceled; return true; }
        
        if(HttpStatusIsNotOk(statusCode, out resultIfProblem)) return true;
        return ErrorCodeIsNotNull(errorCode, out resultIfProblem);
    }
    private bool HttpStatusIsNotOk(HttpStatusCode statusCode, out DownloadingResult? resultIfItIsNotOk)
    {
        switch (statusCode)
        {
            case HttpStatusCode.OK:
                resultIfItIsNotOk = null;
                return false;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.InternalServerError:    
                //TODO: log
                resultIfItIsNotOk = DownloadingResult.HttpRequestErrorCodeReceived;
                return true;
            default:
                throw new InvalidEnumArgumentException("Not processed http status code: " + statusCode);
        }
    }

    private bool ErrorCodeIsNotNull(string? errorCode, out DownloadingResult? resultIfItIsNotNull)
    {
        switch (errorCode)
        {
            case null:
                resultIfItIsNotNull = null;
                return false;
            case "AUTH_INVALID":
                //TODO: log
                resultIfItIsNotNull = DownloadingResult.WrongCredentials;
                return true;
            default:
                throw new ArgumentException("Unhandled error code in response content appeared.");
        }
    }

    
    
    private class AuthenticationPayload 
    {
        [JsonPropertyName("username")] 
        public string Username { get; set; }
        [JsonPropertyName("token")] 
        public string Token { get; set; }
    }
    
    private class AuthenticationResponseContent
    {
        [JsonPropertyName("data")]
        public string? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class DatasetSearchPayload
    {
        [JsonPropertyName("datasetName")]
        public string DatasetName { get; set; }
        [JsonPropertyName("spatialFilter")]
        public SpatialFilterMbr SpatialFilter { get; set; }
    }
    
    private class SpatialFilterMbr
    {
        [JsonPropertyName("filterType")] 
        public string FilterType => "mbr";
        [JsonPropertyName("upperRight")]
        public Coordinate UpperRight { get; set; }
        [JsonPropertyName("lowerLeft")]
        public Coordinate LowerLeft { get; set; }
        
    }

    private class Coordinate
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    private class DatasetSearchResponseContent
    {
        [JsonPropertyName("data")]
        public Dataset[]? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class Dataset
    {
        [JsonPropertyName("datasetAlias")]
        public string DatasetAlias { get; set; }
        [JsonPropertyName("collectionName")]
        public string CollectionName { get; set; }
    }

    private class SceneSearchPayload
    {
        [JsonPropertyName("datasetName")]
        public string DatasetName { get; set; }
        [JsonPropertyName("sceneFilter")]
        public SceneFilter SceneFilter { get; set; }
    }

    private class SceneFilter
    {
        [JsonPropertyName("spatialFilter")]
        public SpatialFilterMbr SpatialFilter { get; set; }
    }

    private class SceneSearchResponseContent
    {
        [JsonPropertyName("data")]
        public SceneRecords? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class SceneRecords
    {
        [JsonPropertyName("results")]
        public ResultingScene[] Results { get; set; }
        
        [JsonPropertyName("recordsReturned")]
        public int RecordsReturned { get; set; }
    }

    private class ResultingScene
    {
        [JsonPropertyName("entityId")]
        public string EntityId { get; set; }
        [JsonPropertyName("spatialBounds")]
        public SpatialBounds SpatialBounds { get; set; }
    }

    private class SpatialBounds
    {
        [JsonPropertyName("coordinates")]
        public int[][][] Coordinates { get; set; }
    }

    private class DownloadOptionsPayload
    {
        [JsonPropertyName("datasetName")]
        public string DatasetName { get; set; }
        [JsonPropertyName("entityIds")]
        public List<string> EntityIds { get; set; }
    }

    private class DownloadOptionsResponseContent
    {
        [JsonPropertyName("data")]
        public Product[]? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class Product
    {
        [JsonPropertyName("available")]
        public bool Available { get; set; }
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        [JsonPropertyName("entityId")]
        public string EntityId { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class DownloadRequestPayload
    {
        [JsonPropertyName("downloads")]
        public List<Download> Downloads { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    public class Download
    {
        [JsonPropertyName("entityId")]
        public string EntityId { get; set; }
        [JsonPropertyName("productId")]
        public string ProductId { get; set; }
    }

    private class DownloadRequestResponseContent
    {
        [JsonPropertyName("data")]
        public RequestResults? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class RequestResults
    {
        [JsonPropertyName("preparingDownloads")]
        public PreparingDownload[]? _preparingDownloads { get; set; }
        public PreparingDownload[] PreparingDownloads => _preparingDownloads ?? [];
        [JsonPropertyName("availableDownloads")]
        public AvailableDownload[]? _availableDownloads { get; set; }
        public AvailableDownload[] AvailableDownloads => _availableDownloads ?? [];
        [JsonPropertyName("duplicateProducts")]
        public Dictionary<string, string>? _duplicateProducts { get; set; }
        public Dictionary<string, string> DuplicateProducts => _duplicateProducts ?? new();
        [JsonPropertyName("failed")]
        public Dictionary<string, string>? _failed { get; set; }
        public Dictionary<string, string> Failed => _failed ?? new();
        [JsonPropertyName("newRecords")]
        public Dictionary<string, string>? _newRecords { get; set; }
        public Dictionary<string, string> NewRecords => _newRecords ?? new();
    }

    private class PreparingDownload;

    private class AvailableDownload
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("downloadId")]
        public int DownloadId { get; set; }
    }

    private class DownloadRetrievePayload
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    private class DownloadRetrieveResponseContent
    {
        [JsonPropertyName("data")]
        public MoreDownloadUrls? Data { get; set; }
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }
    }

    private class MoreDownloadUrls
    {
        [JsonPropertyName("available")]
        public AvailableDownload[] Available { get; set; }
        [JsonPropertyName("requested")]
        public AvailableDownload[] Requested { get; set; }
    }

    private class LogoutPayload{}
    private class LogoutResponseContent
    {
        [JsonPropertyName("data")]
        public Object? Data { get; set; }
    }
    
}
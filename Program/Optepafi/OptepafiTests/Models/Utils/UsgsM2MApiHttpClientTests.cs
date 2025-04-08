using Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;
using Optepafi.Models.Utils.Credentials;

namespace OptepafiTests.Models.Utils;

[TestClass]
public class UsgsM2MApiHttpClientTests
{
    [TestMethod]
    public void TestingSrtmDownloading()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, null, out List<string>? downloadedFilesDirs); 
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.Downloaded, result);
    }
    
    [TestMethod]
    public void TestingSrtmDownloadingNoInternet()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, null, out List<string>? downloadedFilesDirs); 
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.UnableConnectToInternet, result);
    }
    
    [TestMethod]
    public void TestingSrtmDownloadingWithWrongToken()
    {
        string username = "MrKukumber";
        string token = "QxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, null, out List<string>? downloadedFilesDirs); 
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.WrongCredentials, result);
    }
    
    [TestMethod]
    public void TestingSrtmDownloadingWithWrongUserName()
    {
        string username = "MrKukumbe";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, null, out List<string>? downloadedFilesDirs); 
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.WrongCredentials, result);
    }
    
    [TestMethod]
    public void TestingSrtmDownloadingEmptyScenesArray()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [];
        var result = UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, null, out List<string>? downloadedFilesDirs); 
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.Downloaded, result);
    }

    [TestMethod]
    public void TestingSrtmDownloadingCancellingWhileDownloading()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        
        CancellationTokenSource cts = new();
        var cancellationToken = cts.Token;
        
        Task<UsgsM2MApiHttpClient.DownloadingResult> result = Task.Run(() => UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, cancellationToken, out List<string>? downloadedFilesDirs));
        Thread.Sleep(6000);
        cts.Cancel();
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.Canceled, result.Result);
        
    }
    
    [TestMethod]
    public void TestingSrtmDownloadingCancellingWhileRequesting()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        
        CancellationTokenSource cts = new();
        var cancellationToken = cts.Token;
        
        Task<UsgsM2MApiHttpClient.DownloadingResult> result = Task.Run(() => UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, cancellationToken, out List<string>? downloadedFilesDirs));
        Thread.Sleep(4000);
        cts.Cancel();
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.Canceled, result.Result);
        
    }
    //TODO: test ked je otvoreny zip subor inym threadom a downloading sa donho bude snazit zapisat...nemalo by sa stavat ale nemusela by mi koli tomu spadnut aplikacia
    
    [TestMethod]
    public void TestingSrtmDownloadingWhileBilFileIsOpen()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new(){UserName = username, AuthenticationToken = token};
        (int, int)[] indicesOfScenesToBeDownloaded = [(49,9), (50,10)];
        
        CancellationTokenSource cts = new();
        var cancellationToken = cts.Token;
        
        Task<UsgsM2MApiHttpClient.DownloadingResult> result = Task.Run(() => UsgsM2MApiHttpClient.Instance.RequestAndDownloadSrtm1ArcSecondGlobalDatasetScenes(indicesOfScenesToBeDownloaded, Path.Combine("testDownloadedScenes"), credentials, cancellationToken, out List<string>? downloadedFilesDirs));
        using (FileStream fs = new FileStream(Path.Combine("testDownloadedScenes", "n49_e009_1arc_v3_bil.zip"), FileMode.Open, FileAccess.Write))
        { Thread.Sleep(60000); }
        Assert.AreEqual(UsgsM2MApiHttpClient.DownloadingResult.UnableToWriteDataToFile, result.Result);
    }
}
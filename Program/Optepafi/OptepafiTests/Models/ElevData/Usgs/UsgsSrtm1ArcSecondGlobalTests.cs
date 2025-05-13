using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Credentials;

namespace OptepafiTests.Models.ElevData.Usgs;


[TestClass]
public class UsgsSrtm1ArcSecondGlobalTests
{
    [TestMethod]
    public void UsgsSrtm1ArcSecondGlobalDownloadSlovakia()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new Credentials { UserName = username, AuthenticationToken = token };
        UsgsSrtm1ArcSecondGlobal.Instance.Initialize();
        var result = UsgsSrtm1ArcSecondGlobal.Instance.Download(UsgsSrtm1ArcSecondGlobal.Instance.AllTopRegions.First(), credentials, null);
        Assert.AreEqual(ElevDataManager.DownloadingResult.Downloaded, result);
    }
    
    [TestMethod]
    public void UsgsSrtm1ArcSecondGlobalRemoveSlovakia()
    {
        string username = "MrKukumber";
        string token = "IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA";
        Credentials credentials = new Credentials { UserName = username, AuthenticationToken = token };
        UsgsSrtm1ArcSecondGlobal.Instance.Initialize();
        UsgsSrtm1ArcSecondGlobal.Instance.Remove(UsgsSrtm1ArcSecondGlobal.Instance.AllTopRegions.First());
    }

    [TestMethod]
    public void UsgsSrtm1ArcSecondGlobalRetrieveDataForMap()
    {
        TestMap map = new(new GeoCoordinates(18, 48), new GeoCoordinates(17.5, 47.5), new GeoCoordinates(18.5, 48.5));
        UsgsSrtm1ArcSecondGlobal.Instance.Initialize();
        ElevDataManager.ElevDataObtainability areObtainable = UsgsSrtm1ArcSecondGlobal.Instance.AreElevDataObtainableFor(map, null);
        if (areObtainable is ElevDataManager.ElevDataObtainability.Obtainable)
        {
            var elevData = UsgsSrtm1ArcSecondGlobal.Instance.GetElevDataFor(map, null);
        }
    }

    private class TestMap(GeoCoordinates repreLoc, GeoCoordinates swBoundCor, GeoCoordinates neBoundCor) : IBoxBoundedGeoLocMap
    {
        public string FileName { get; init; } = "";
        public string FilePath { get; init; } = "";
        public int Scale { get; } = 1;

        public TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
        public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)=> genericVisitor.GenericVisit(this);
        public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

        public GeoCoordinates RepresentativeLocation { get; } = repreLoc;

        public GeoCoordinates SouthWestBoundingCorner { get; } = swBoundCor;
        public GeoCoordinates NorthEastBoundingCorner { get; } = neBoundCor;
    }
}
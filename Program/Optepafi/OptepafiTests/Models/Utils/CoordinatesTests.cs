using Optepafi.Models.Utils;

namespace OptepafiTests.Models.Utils;

[TestClass]
public class CoordinatesTests
{
    [TestMethod]
    public void TestGeoToMapNegative()
    {
        GeoCoordinates coord = new(47, 16);
        GeoCoordinates reference = new(48, 17);
        MapCoordinates result = coord.ToMapCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestGeoToMapPositive()
    {
        GeoCoordinates coord = new(49, 18);
        GeoCoordinates reference = new(48, 17);
        MapCoordinates result = coord.ToMapCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestGeoToMapLongitudeOnEquatorPositive()
    {
        GeoCoordinates coord = new(49, 0);
        GeoCoordinates reference = new(48, 0);
        MapCoordinates result = coord.ToMapCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestGeoToMapOverTheEdgePositive()
    {
        GeoCoordinates coord = new(-179, 17);
        GeoCoordinates reference = new(179, 17);
        MapCoordinates result = coord.ToMapCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestGeoToMapOverThePole()
    {
        GeoCoordinates coord = new(46, 89);
        GeoCoordinates reference = new(45, 89);
        MapCoordinates result = coord.ToMapCoordinates(reference, 1_000_000);
    }
    
    
    [TestMethod]
    public void TestMapToGeoNegative()
    {
        MapCoordinates coord = new(-106336, -111940);
        GeoCoordinates reference = new(48, 17);
        GeoCoordinates result = coord.ToGeoCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestMapToGeoPositive()
    {
        MapCoordinates coord = new(106336, 111940);
        GeoCoordinates reference = new(48, 17);
        GeoCoordinates result = coord.ToGeoCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestMapToGeoXPosOnEquatorPositive()
    {
        MapCoordinates coord = new(111940, 0);
        GeoCoordinates reference = new(48, 0);
        GeoCoordinates result = coord.ToGeoCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestMapToGeoOverTheEdgePositive()
    {
        MapCoordinates coord = new(106336 * 2, 0);
        GeoCoordinates reference = new(179, 17);
        GeoCoordinates result = coord.ToGeoCoordinates(reference, 1_000_000);
    }
    
    [TestMethod]
    public void TestMapToGeoOverThePole()
    {
        MapCoordinates coord = new(0, 111940 * 2);
        GeoCoordinates reference = new(48, 89);
        GeoCoordinates result = coord.ToGeoCoordinates(reference, 1_000_000);
    }
}
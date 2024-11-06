using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.MapInterfaces;

namespace OptepafiTests.Models.Graphics.Omap;

[TestClass]
public class OmapGraphicsAggregatorTests
{
    private string _testFilesPath = Path.Combine("test_files", "models", "graphics", "omap");
    
    [TestMethod]
    public void ClassicTest()
    {
        MapManager.MapCreationResult? result;
        IMap? map;
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_more_objects.omap"), FileMode.Open, FileAccess.Read))
        {
            result = MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_notLoc.omap")),
                OmapMapRepresentative.Instance, null, out map);
        }
        
        if (result is MapManager.MapCreationResult.Ok || result is MapManager.MapCreationResult.Incomplete)
        {
            SimpleCollector collector = new SimpleCollector();
            GraphicsManager.Instance.AggregateMapGraphics(map!, collector, null);
        }
        
    }
    
    
}
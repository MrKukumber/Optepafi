using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.MapInterfaces;

namespace OptepafiTests.Models.Maps.Omap;

[TestClass]
public class OmapTests
{
    private string _testFilesPath = Path.Combine("test_files", "models", "maps", "omap");
    
    [TestMethod]
    public void TestNotLocal()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_notLoc.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_notLoc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }

    [TestMethod]
    public void TestLocal()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_loc.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_loc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }

    [TestMethod]
    public void TestDamagedXml()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_damaged.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_loc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }

    [TestMethod]
    public void TestMoreObjectsMap()
    { 
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_more_objects.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_loc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }

    [TestMethod]
    public void TestKarpatyMap()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_karpaty.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_loc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }

    [TestMethod]
    public void TestSymbolMissing()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "test_map_missing_contour_symbol.omap"), FileMode.Open, FileAccess.Read))
        {
            MapManager.Instance.TryGetMapFromOf((fs, Path.Combine(_testFilesPath, "test_map_loc.omap")),
                OmapMapRepresentative.Instance, null, out IMap? map);
        }
    }
}
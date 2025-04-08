using Optepafi.Models.Utils.EsriBilParser;

namespace OptepafiTests.Models.Utils;

[TestClass]
public class EsrilBilParserTests
{
    [TestMethod]
    public void TestParsing()
    {
        // var path = Path.Combine("test_esri_bil", "n59_e018_1arc_v3_bil", "n59_e018_1arc_v3.bil");
        var path = Path.Combine("test_esri_bil", "n00_e009_1arc_v3_bil", "n00_e009_1arc_v3.bil");
        var ebp = new EsriBilParser();
        ebp.ReadBilHeader(path);
        int[][,] bytes = ebp.GetAllBandsInts();
        Console.WriteLine(bytes.Length);
        
    }
    
}
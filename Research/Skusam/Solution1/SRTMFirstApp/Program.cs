using SRTM;
using SRTM.Sources.NASA;
using SRTM.Sources.USGS;


namespace SRTMFirstApp;

public class Program
{
    public static void Main(string[] args)
    {
        ISRTMData data = new SRTMData("", new USGSSource());
        data.
    }   
}
using System.Collections.Generic;
using Optepafi.Models.MapRepre;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Static class for 
/// </summary>
public static class MapManager
{
    public static List<IMapFormat> AllMapFormats { get; } = new List<IMapFormat>();

    public static List<IMapFormat> GetCorrespondingFormatsTo(List<> mapRepres)
    {
        //TODO
        return null;
    }
}
using System.IO;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.MapMan.MapFormats;

public sealed class OMAPFormat : IMapFormat<OMAP>
{
    private OMAPFormat(){}
    static public OMAPFormat Instance { get; } = new OMAPFormat();
    
    //TODO: implement
    public string Extension { get; }
    public string MapFormatName { get; }
    public OMAP? CreateMapFrom(Stream inputMapStream, out MapManager.MapCreationResult creationResult)
    {
        throw new System.NotImplementedException();
    }
}
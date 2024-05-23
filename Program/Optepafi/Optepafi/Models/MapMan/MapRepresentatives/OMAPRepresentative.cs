using System.IO;
using System.Threading;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.MapMan.MapFormats;

public sealed class OMAPRepresentative : IMapRepresentative<OMAP>
{
    private OMAPRepresentative(){}
    static public OMAPRepresentative Instance { get; } = new OMAPRepresentative();
    
    //TODO: implement
    public string Extension { get; }
    public string MapFormatName { get; }
    public OMAP? CreateMapFrom((Stream,string) inputMapStreamWithPath, CancellationToken? cancellationToken, out MapManager.MapCreationResult creationResult)
    {
        throw new System.NotImplementedException();
    }
}
using System.IO;

namespace Optepafi.Models.MapMan;

public interface IMapFormat
{
    string Extension { get; }
    string MapFormatName { get; }

    IMap CreateMap(StreamReader inputFile);

}
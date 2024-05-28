using System.IO;
using System.Threading;
using Optepafi.Models.MapMan.Maps;

namespace Optepafi.Models.MapMan.MapFormats;

public class TextMapRepresentative : IMapRepresentative<TextMap>
{
    public static TextMapRepresentative Instance { get; } = new();
    private TextMapRepresentative() { }

    public string MapFormatName { get; } = "Text map";
    public string Extension { get; } = "txt";

    public TextMap? CreateMapFrom((Stream, string) inputMapStreamWithPath, CancellationToken? cancellationToken,
        out MapManager.MapCreationResult creationResult)
    {
        var (inputMapStream, path) = inputMapStreamWithPath;
        string mapText = "";
        using (var sr = new StreamReader(inputMapStream))
        {
            mapText = sr.ReadToEnd();
        }

        IntraTextMap textMap = new IntraTextMap()
        {
            FileName = Path.GetFileName(path),
            FilePath = path,
            _text = mapText,
        };
        creationResult = MapManager.MapCreationResult.Ok;
        return textMap;
    }

    private class IntraTextMap : TextMap
    {
        public string _text = "";
        public override string Text => _text;
    }
}
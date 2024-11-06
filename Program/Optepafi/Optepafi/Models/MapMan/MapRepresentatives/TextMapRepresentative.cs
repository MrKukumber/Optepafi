using System.Collections.Generic;
using System.IO;
using System.Threading;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.SearchingAlgorithmMan;

namespace Optepafi.Models.MapMan.MapRepresentatives;

/// <summary>
/// Represents <c>TextMap</c> type in application.
/// 
/// It is included in <c>MapManager</c> where indicates usability of this type.  
/// For more information on map representatives see <see cref="IMapRepresentative{TMap}"/>, <see cref="IMapFormat{TMap}"/> or <see cref="IMapIdentifier{TMap}"/> class.  
/// </summary>
public class TextMapRepresentative : IMapRepresentative<TextMap>
{
    public static TextMapRepresentative Instance { get; } = new();
    private TextMapRepresentative() { }

    /// <inheritdoc cref="IMapFormat{TMap}.MapFormatName"/>
    public string MapFormatName { get; } = "Text map";
    
    /// <inheritdoc cref="IMapFormat{TMap}.Extension"/>
    public string Extension { get; } = "txt";

    
    /// <inheritdoc cref="IMapFormat{TMap}.CreateMapFrom"/>
    /// <remarks>
    /// Reads input text file and saves its text into <c>TextMap</c> instance and returns it.  
    /// It do it so by creating instance of hidden <c>IntraTextMap</c> class which has public backing field for text.
    /// </remarks>
    public TextMap CreateMapFrom((Stream mapStream, string path) input, CancellationToken? cancellationToken,
        out MapManager.MapCreationResult creationResult)
    {
        string mapText;
        using (var sr = new StreamReader(input.mapStream))
        {
            mapText = sr.ReadToEnd();
        }

        IntraTextMap textMap = new IntraTextMap
        {
            FileName = Path.GetFileName(input.path),
            FilePath = input.path,
            text = mapText
        };
        creationResult = MapManager.MapCreationResult.Ok;
        return textMap;
    }
    /// <inheritdoc cref="IMapIdentifier{TMap}.GetDefaultTrackFrom"/> 
    public List<Leg>? GetDefaultTrackFrom(TextMap map) => null;

    /// <summary>
    /// Hidden intra class which inherits from <c>TextMap</c>. It contains public backing field of <c>Text</c> property so it could be set from <c>CreateMapFrom</c> method.
    /// </summary>
    private class IntraTextMap : TextMap
    {
        public string text = "";
        public override string Text => text;
    }
}


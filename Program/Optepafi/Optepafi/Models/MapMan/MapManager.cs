using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapMan.MapRepresentatives;

namespace Optepafi.Models.MapMan;

/// <summary>
/// Singleton class used for managing maps that are used in creation of map representations. It contains set of all supported map formats. It is main channel between operations on maps and applications logic (ModelViews/ViewModels). 
/// It implements supporting methods for operating with maps. They should be managed preferably trough this class.
/// All operations provided by this class are thread safe as long as same method arguments are not used concurrently multiple times.
/// </summary>
public class MapManager : IMapGenericVisitor<IMapFormat<IMap>>
{
    public static MapManager Instance { get; } = new();
    private MapManager(){}
    
    /// <summary>
    /// Set of all usable map formats in application.
    /// </summary>
    public ISet<IMapFormat<IMap>> MapFormats { get; } = ImmutableHashSet.Create<IMapFormat<IMap>>(TextMapRepresentative.Instance);

    /// <summary>
    /// Returns map format for a specified map. It do it so by use of "generic visitor pattern" on map.
    /// After visiting of map it runs trough all usable map formats and pattern-matches them to map identifier with desired map type parameter.
    /// If none map format matches to provided map, exception is thrown. It namely means that map was not created by any of those map formats that is against principle of usage of these constructs.
    /// </summary>
    /// <param name="map">Map which format is looked for.</param>
    /// <returns>Format of provided map.</returns>
    public IMapFormat<IMap> GetFormat(IMap map)
    {
        return map.AcceptGeneric(this);
    }
    IMapFormat<IMap> IMapGenericVisitor<IMapFormat<IMap>>.GenericVisit<TMap>(TMap map) 
    {
        foreach (var mapFormat in MapFormats)
        {
            if (mapFormat is IMapIdentifier<TMap>)
                return mapFormat;
        }
        throw new ArgumentException("Given map was not created by any existing map format.");
    }

    /// <summary>
    /// Returns map format, whose file extension matches with extension of provided file name.
    /// </summary>
    /// <param name="mapFileName">Name of file for which corresponding map forma should be returned.t</param>
    /// <returns>Corresponding map format to provided file name. If there is no matching map format, returns null.</returns>
    public IMapFormat<IMap>? GetCorrespondingMapFormatTo(string mapFileName)
    {
        foreach (var mapFormat in MapFormats)
        {
            if (Regex.IsMatch(mapFileName, ".*\\." + mapFormat.Extension + "$") ) return mapFormat;
        }
        return null;
    }
    public enum MapCreationResult{Ok, Incomplete, UnableToParse, Cancelled}
    /// <summary>
    /// Used for calling map format parser on a provided map stream and returning corresponding map object.
    /// Parsing of map should be done swiftly in linear time complexity given by size of file.
    /// </summary>
    /// <param name="mapStreamWithPath">Provided stream from which map should be parsed. It comes with pat to the source file of the stream.</param>
    /// <param name="mapFormat">Map format by which should be map parsed.</param>
    /// <param name="cancellationToken">Token for cancellation of parsing.</param>
    /// <param name="map">Out parameter for retrieving parsed map.</param>
    /// <returns>Result of map creation.</returns>
    public MapCreationResult TryGetMapFromOf((Stream,string) mapStreamWithPath, IMapFormat<IMap> mapFormat, CancellationToken? cancellationToken, out IMap? map)
    {
        map =  mapFormat.CreateMapFrom(mapStreamWithPath, cancellationToken, out MapCreationResult creationResult);
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return MapCreationResult.Cancelled;
        return creationResult;
    }


}
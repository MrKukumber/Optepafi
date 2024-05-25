using System;
using System.IO;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

/// <summary>
/// One of three interfaces whose implementations represent individual map formats.
/// The other two are <see cref="IMapRepresentative{TMap}"/> and <see cref="IMapIdentifier{TMap}"/>.
/// This interface provides methods and properties, that are used for specification of map formats and creation of map objects.
/// It should not be implemented right away. All implementations should implement <c>IMapRepresentative{TMap}</c> instead.
/// Thanks to covariance of its map type parameter it is useful for transferring of map representatives in non generic way.
/// </summary>
/// <typeparam name="TMap">Type of represented map.</typeparam>
public interface IMapFormat<out TMap> where TMap : IMap
{
    string MapFormatName { get; }
    
    /// <summary>
    /// File extension of represented maps format.
    /// </summary>
    string Extension { get; }
    
    /// <summary>
    /// Tries to parse provided stream into the map object.
    /// This parsing should be done in linear time complexity relatively to size of the map file stream.
    /// </summary>
    /// <param name="inputMapStreamWithPath">Provided map file stream intended to be parsed. It comes along side with path to the source file</param>
    /// <param name="cancellationToken">Token for cancellation of parsing.</param>
    /// <param name="creationResult">Out parameter for result of map creation.</param>
    /// <returns>Resulting created map object.</returns>
    TMap? CreateMapFrom((Stream,string) inputMapStreamWithPath, CancellationToken? cancellationToken, out MapManager.MapCreationResult creationResult);
}

using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

/// <summary>
/// One of three interfaces which implementations represent individual map formats.
/// The other two interfaces are <see cref="IMapRepresentative{TMap}"/> and <see cref="IMapFormat{TMap}"/>.
/// This interface should not be implemented right away. All implementations should implement <c>IMapRepresentative</c> instead.
/// Thanks to contravariant nature of its map type parameter it is useful for correct pattern matching and identifying map representatives.
/// </summary>
/// <typeparam name="TMap">Type of represented map.</typeparam>
public interface IMapIdentifier<in TMap> where TMap : IMap;
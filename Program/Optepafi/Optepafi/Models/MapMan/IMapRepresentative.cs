using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

/// <summary>
/// One of three interfaces whose implementations represent individual map formats.
/// The other two are <see cref="IMapFormat{TMap}"/> and <see cref="IMapIdentifier{TMap}"/>. From these three interfaces this is the one which should be implemented by map representatives.
/// Every type of map should have its representative, so it could be found as viable map type in <see cref="MapManager"/> class.
/// Preferred way to interact with representatives is through <see cref="MapManager"/>.
/// </summary>
/// <typeparam name="TMap">Type of represented map objects.</typeparam>
public interface IMapRepresentative<TMap> : IMapFormat<TMap>, IMapIdentifier<TMap>
    where TMap : IMap;
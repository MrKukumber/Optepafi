namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents ability of map to be located using geographic coordinate system.
/// This localization can be achieved by just providing geo reference of origin of maps coordination system or by providing some representative maps <see cref="GeoCoordinate"/>.
/// This interface should be implemented for example in case, when we want to get corresponding elevation data for specific map.
/// </summary>
public interface IGeoLocatedMap : IMap
{
    public GeoCoordinate RepresentativeLocation { get; }
}
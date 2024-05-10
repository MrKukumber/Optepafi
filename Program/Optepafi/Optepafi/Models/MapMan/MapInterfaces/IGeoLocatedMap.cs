namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IGeoLocatedMap : IMap
{
    public GeoCoordinate RepresentativeLocation { get; }
}
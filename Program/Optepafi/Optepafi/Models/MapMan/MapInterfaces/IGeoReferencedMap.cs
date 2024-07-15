namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represent map, which uses its own coordinate system for describing map objects, but also contains GCS reference of ist coordination systems origin.
/// 
/// This geo-reference is set to be the representative location of <see cref="IGeoLocatedMap"/> of the map.
/// </summary>
public interface IGeoReferencedMap : IGeoLocatedMap
{
    public GeoCoordinate GeoReference { get => RepresentativeLocation; }

    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams, TMapConstraint>(
        // IMapGenericVisitor<TOut, TConstraint, TOtherParams, TMapConstraint> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams)
        // where TGenericParam : TConstraint
        // where TMapConstraint : IGeoReferencedMap;
}
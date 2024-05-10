using Optepafi.Models.ElevationDataMan;

namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IGeoReferencedMap : IGeoLocatedMap
{
    public GeoCoordinate GeoReference { get => RepresentativeLocation; }

    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams, TMapConstraint>(
        // IMapGenericVisitor<TOut, TConstraint, TOtherParams, TMapConstraint> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams)
        // where TGenericParam : TConstraint
        // where TMapConstraint : IGeoReferencedMap;
}
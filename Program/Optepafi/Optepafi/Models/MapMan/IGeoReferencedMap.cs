using Optepafi.Models.ElevationDataMan;

namespace Optepafi.Models.MapMan;

public interface IGeoReferencedMap : IMap
{
    public GeoCoordinate GeoReference { get; }

    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams, TMapConstraint>(
        // IMapGenericVisitor<TOut, TConstraint, TOtherParams, TMapConstraint> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams)
        // where TGenericParam : TConstraint
        // where TMapConstraint : IGeoReferencedMap;
}
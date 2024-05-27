using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

public class GeoReferenceContainingOMAP : OMAP, IGeoReferencedMap
{
    //TODO: implement
    public GeoCoordinate RepresentativeLocation { get; }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        throw new System.NotImplementedException();
    }
}
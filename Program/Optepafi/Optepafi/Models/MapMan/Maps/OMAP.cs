using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public class OMAP : IGeoReferencedMap
{
    public TOut AcceptGeneric<TOut, TSomeone, TConstraint, TOtherParams, TGeoReferenceMap>(
        IMapGenericVisitor<TOut, TConstraint, TOtherParams, TGeoReferenceMap> genericVisitor,
        TSomeone genericParam, TOtherParams otherParams)
        where TSomeone : TConstraint
        where TGeoReferenceMap : OMAP

    {
        return genericVisitor.GenericVisit(this, genericParam, otherParams);
    }
    // public TOut AcceptGeneric<TOut, TSomeone, TSomeonesConstraint, TOtherParams>(
    // IGeoReferencedMapGenericVisitor<TOut, TSomeonesConstraint, TOtherParams> genericVisitor,
    // TSomeone genericParam, TOtherParams otherParams) where TSomeone : TSomeonesConstraint
    // {
    // return genericVisitor.GenericVisit(this, genericParam, otherParams);
    // }
}
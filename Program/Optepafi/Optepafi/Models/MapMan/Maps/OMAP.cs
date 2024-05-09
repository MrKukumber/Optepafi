using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public class OMAP : IGeoReferencedMap
{
    public TOut AcceptGeneric<TOut, TSomeone, TConstraint, TOtherParams>(
        IMapGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TSomeone genericParam, TOtherParams otherParams)
        where TSomeone : TConstraint
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
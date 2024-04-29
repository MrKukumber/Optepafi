using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public sealed class OMAP : IMap
{
    public IMapFormat<IMap> MapFormat { get; init; }

    public TOut AcceptGeneric<TOut, TSomeone, TSomeonesConstraint, TOtherParams>(
        IMapGenericVisitor<TOut, TSomeonesConstraint, TOtherParams> genericVisitor,
        TSomeone genericParam, TOtherParams otherParams) where TSomeone : TSomeonesConstraint
    {
        return genericVisitor.GenericVisit(this, genericParam, otherParams);
    }
}
using System;
using Optepafi.Models.MapMan.MapFormats;

namespace Optepafi.Models.MapMan.Maps;

public sealed class OMAP : IMap
{
    public IMapFormat<IMap> MapFormat { get; init; }

    public TOut AcceptGenericWithSomeone<TOut, TSomeone, TSomeonesConstraint, TOtherParams>(
        IMapGenericVisitorWithSomeone<TOut, TSomeonesConstraint, TOtherParams> genericVisitorWithSomeone,
        TSomeone someone, TOtherParams otherParams) where TSomeone : TSomeonesConstraint
    {
        return genericVisitorWithSomeone.GenericVisit(this, someone, otherParams);
    }
}
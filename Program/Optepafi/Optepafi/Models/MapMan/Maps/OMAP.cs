using System;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

public class OMAP : IMap
{
    //TODO: implement
    public string Name { get; }
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        throw new NotImplementedException();
    }

    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    {
        throw new NotImplementedException();
    }
}
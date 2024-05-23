using System;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

public class OMAP : IMap
{
    //TODO: implement
    public string FileName { get; init; }
    public required string FilePath { get; init; }
    public virtual TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        throw new NotImplementedException();
    }

    public virtual TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    {
        throw new NotImplementedException();
    }
}
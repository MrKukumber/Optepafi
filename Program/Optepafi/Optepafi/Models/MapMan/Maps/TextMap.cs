using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

public abstract class TextMap : IMap
{
    public required string FileName { get; init; }
    public required string FilePath { get; init; }
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }
    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }
    
    public abstract string Text { get; }
}
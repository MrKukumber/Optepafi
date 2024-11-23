using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.Maps;

//TODO: comment
public abstract class OmapMap : IMap, IPartitionableMap
{
    //TODO: implement
    public string FileName { get; init; }
    public required string FilePath { get; init; }
    
    public virtual TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public virtual TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    { return genericVisitor.GenericVisit(this); }
    
    
    public abstract int Scale { get; }
    public abstract Dictionary<decimal, List<Object>> Objects { get; }
    public abstract List<Symbol> Symbols { get; }
    public record struct Symbol(decimal Code);
    public record struct Object((MapCoordinates, byte)[] TypedCoords, float SymbolRotation);
    
    public abstract MapCoordinates NorthernmostCoords { get; }
    public abstract MapCoordinates SouthernmostCoords { get; }
    public abstract MapCoordinates WesternmostCoords { get; }
    public abstract MapCoordinates EasternmostCoords { get; }

    public abstract IMap GetPartitionOfSize(int size, CancellationToken? cancellationToken, out bool wholeMapReturned);
}

public abstract class GeoLocatedOmapMap : OmapMap, IMostNSWECoordQueryableGeoRefMap
{
    //TODO: implement
    public abstract GeoCoordinates RepresentativeLocation { get; }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public override TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public override TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    { return genericVisitor.GenericVisit(this); }
}


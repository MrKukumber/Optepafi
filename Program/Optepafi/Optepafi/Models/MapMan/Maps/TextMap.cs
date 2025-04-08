using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.Maps;

/// <summary>
/// Represents map generated from any text file. It contains text of given text file.
/// 
/// This is just demonstrative class for presenting applications functionality.  
/// For more information about map classes see <see cref="IMap"/>.  
/// For more information on geo-located map classes see <see cref="IGeoLocatedMap"/>.  
/// </summary>
public abstract class TextMap : IBoxBoundedGeoRefMap
{
    /// <inheritdoc cref="IMap.FileName "/>
    public required string FileName { get; init; }
    
    /// <inheritdoc cref="IMap.FilePath"/>
    public required string FilePath { get; init; }
    
    /// <inheritdoc cref="IMap.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }
    /// <inheritdoc cref="IMap.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit(this);
    }

    /// <inheritdoc cref="IGeoLocatedMap.AcceptGeneric{TOut,TOtherParams}(Optepafi.Models.MapMan.IGeoLocatedMapGenericVisitor{TOut,TOtherParams},TOtherParams)"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }
    
    
    /// <inheritdoc cref="IGeoLocatedMap.RepresentativeLocation"/>
    /// <summary>
    /// Every text map is located in Prague, on the river side.
    /// </summary>
    public GeoCoordinates RepresentativeLocation { get; } = new GeoCoordinates(50.088391, 14.413755);
    
    /// <inheritdoc cref="IBoxBoundedGeoRefMap.BottomLeftBoundingCorner"/>
    /// <remarks>
    /// Height of every text map is 100000 micrometers, and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinates BottomLeftBoundingCorner { get; } = new MapCoordinates(-50000, -50000);

    /// <inheritdoc cref="IBoxBoundedGeoRefMap.TopRightBoundingCorner"/>
    /// <remarks>
    /// Height of every text map is 100000 micrometers, and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinates TopRightBoundingCorner { get; } = new MapCoordinates(50000, 50000);
    
    public int Scale { get; } = 10_000;

    /// <summary>
    /// Text of represented text file.
    /// </summary>
    public abstract string Text { get; }
}
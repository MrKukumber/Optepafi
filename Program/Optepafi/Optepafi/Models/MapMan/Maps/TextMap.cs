using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

/// <summary>
/// Represents map generated from any text file. It contains text of given text file.
/// 
/// This is just demonstrative class for presenting applications functionality.  
/// For more information about map classes see <see cref="IMap"/>.  
/// For more information on geo-located map classes see <see cref="IGeoLocatedMap"/>.  
/// </summary>
public abstract class TextMap : IMostNSWECoordQueryableGeoRefMap
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
    public GeoCoordinate RepresentativeLocation { get; } = new GeoCoordinate(50.088391, 14.413755);

    /// <inheritdoc cref="IMostNSWECoordQueryableGeoRefMap.NorthernmostCoords"/>
    /// <remarks>
    /// Height of every text map is 100000 micrometers and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinate NorthernmostCoords { get; } = new MapCoordinate(0, 50000);
    
    /// <inheritdoc cref="IMostNSWECoordQueryableGeoRefMap.SouthernmostCoords"/>
    /// <remarks>
    /// Height of every text map is 100000 micrometers and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinate SouthernmostCoords { get; } = new MapCoordinate(0, -50000);
    
    /// <inheritdoc cref="IMostNSWECoordQueryableGeoRefMap.WesternmostCoords"/>
    /// <remarks>
    /// Width of every text map is 100000 micrometers and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinate WesternmostCoords { get; } = new MapCoordinate(-50000, 0);
    
    /// <inheritdoc cref="IMostNSWECoordQueryableGeoRefMap.EasternmostCoords"/>
    /// <remarks>
    /// Width of every text map is 100000 micrometers and it is centered on coordinate (0,0).
    /// </remarks>
    public MapCoordinate EasternmostCoords { get; } = new MapCoordinate(50000, 0);

    /// <summary>
    /// Text of represented text file.
    /// </summary>
    public abstract string Text { get; }
}
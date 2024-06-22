using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

/// <summary>
/// Represents map generated from any text file. It contains text of given text file.
/// This is just demonstrative class for presenting applications functionality.
/// For more information about map classes see <see cref="IMap"/>.
/// </summary>
public abstract class TextMap : IMap
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
    
    /// <summary>
    /// Text of represented text file.
    /// </summary>
    public abstract string Text { get; }
}
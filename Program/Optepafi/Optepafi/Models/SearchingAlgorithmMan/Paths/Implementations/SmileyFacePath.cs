using Optepafi.Models.MapMan;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths.Implementations;

public record class SmileyFacePath(MapCoordinate StartPosition, MapCoordinate EndPosition) : IPath
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit(this, otherParams);
    }
}
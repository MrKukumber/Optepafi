namespace Optepafi.Models.SearchingAlgorithmMan.Paths;

public class ClasicColoredPath : IPath
{
    //TODO:
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<ClasicColoredPath>(this, otherParams);
    }
}
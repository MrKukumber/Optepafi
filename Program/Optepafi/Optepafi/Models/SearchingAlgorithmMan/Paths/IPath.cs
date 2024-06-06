namespace Optepafi.Models.SearchingAlgorithmMan.Paths;

public interface IPath
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
}
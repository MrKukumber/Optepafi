namespace Optepafi.Models.SearchingAlgorithmMan.Paths;


public interface IPathGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TPath>(TPath path, TOtherParams otherParams)
        where TPath : IPath;
}
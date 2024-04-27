namespace Optepafi.Models.MapMan;

public interface IMapGenericVisitorWithSomeone<TOut, TSomeonesConstraint, TOtherParams>
{
    public TOut GenericVisit<TMap, TSomeone>(TMap map, TSomeone someone,
        TOtherParams otherParams)
        where TMap : IMap
        where TSomeone : TSomeonesConstraint;
}
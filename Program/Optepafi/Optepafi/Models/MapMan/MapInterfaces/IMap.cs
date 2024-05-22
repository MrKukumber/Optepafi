namespace Optepafi.Models.MapMan.MapInterfaces;

public interface IMap
{
    string Name { get; }


    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        // IMapGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams)
        // where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(
        IMapGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor);
    // public void AcceptGeneric(IMapGenericVisitor genericVisitor);

}

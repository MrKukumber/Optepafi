using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

// public interface IMapGenericVisitor<TOut, TConstraint, TOtherParams>
// {
    // public TOut GenericVisit<TMap, TGenericParam>(TMap map, TGenericParam genericParam,
        // TOtherParams otherParams)
        // where TMap : IMap
        // where TGenericParam : TConstraint;
// }

public interface IMapGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TMap>(TMap map, TOtherParams otherParams)
        where TMap : IMap;
}

// public interface IMapGenericVisitor<TOut>
// {
    // public TOut GenericVisit<TMap>(TMap map) where TMap : IMap;
// }

// public interface IMapGenericVisitor
// {
    // public void GenericVisit<TMap>(TMap map);
// }

using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

// public interface IMapGenericVisitor<TOut, TConstraint, TOtherParams>
// {
    // public TOut GenericVisit<TMap, TGenericParam>(TMap map, TGenericParam genericParam,
        // TOtherParams otherParams)
        // where TMap : IMap
        // where TGenericParam : TConstraint;
// }

/// <summary>
/// On of generic visitor interfaces for <see cref="IMap"/> implementations. It provides access to modified visitor pattern on maps, where only one generic method is required to be implemented.
/// It serves mainly for acquiring generic parameter, that represents real type of visited map.
/// It has one more overload for convenience of use.
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of GenericVisit.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface IMapGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="map">Map, which accepted the visit.</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TMap">Type of accepting map. Main result of this modified visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut GenericVisit<TMap>(TMap map, TOtherParams otherParams)
        where TMap : IMap;
}
/// <summary>
/// One of generic visitor interfaces for <see cref="IMap"/> implementations. It provides acces to modified visitor pattern on maps.
/// For more information see <see cref="IMapGenericVisitor{TOut,TOtherParams}"/> .
/// </summary>
public interface IMapGenericVisitor<TOut>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information of this method see <see cref="IMapGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    public TOut GenericVisit<TMap>(TMap map) where TMap : IMap;
}


// public interface IMapGenericVisitor
// {
    // public void GenericVisit<TMap>(TMap map);
// }

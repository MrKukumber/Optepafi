using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

// public interface IGeoLocatedMapGenericVisitor<TOut, TConstraint, TOtherParams>
// {
    // public TOut GenericVisit<TMap, TGenericParam>(TMap map, TGenericParam genericParam,
        // TOtherParams otherParams)
        // where TMap : 
        // where TGenericParam : TConstraint;
// }



/// <summary>
/// Generic visitor interfaces for <see cref="IMap"/> implementations.
/// 
/// It provides access to modified visitor pattern on maps, where only one generic method is required to be implemented.  
/// It serves mainly for acquiring generic parameter, that represents real type of visited map.  
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of <c>GenericVisit</c>.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface IGeoLocatedMapGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="geoLocatedMap">Geo-located map which accepted the visit</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TGeoLocatedMap">Type of accepting geo-located map. Main result of this modified visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut GenericVisit<TGeoLocatedMap>(TGeoLocatedMap geoLocatedMap, TOtherParams otherParams)
        where TGeoLocatedMap : IGeoLocatedMap;
}
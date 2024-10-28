using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Represents ability of map to be located using geographic coordinate system.
/// 
/// This localization can be achieved by just providing geo reference of origin of maps coordination system or by providing some representative maps <see cref="GeoCoordinate"/>.  
/// This interface should be implemented for example in case, when we want to get corresponding elevation data for specific map.  
///
/// It provides modification of visitor pattern, so-called "generic visitor pattern".  
/// The main goal is not ensuring that caller implements specific method for every <c>IGeoLocatedMap</c> implementation, but for ability to retrieve maps real type in form of type parameter.  
/// </summary>
public interface IGeoLocatedMap : IMap
{
    public GeoCoordinate RepresentativeLocation { get; }
    
    /// <summary>
    /// Method used in generic visitor pattern of <c>IGeoLocatedMap</c> interface.
    /// 
    /// For more information on this pattern see <see cref="IGeoLocatedMapGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor ;).</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of return value carried through visitor pattern.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of the rest of parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
}
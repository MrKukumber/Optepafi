
using Optepafi.Models.MapRepreMan.MapRepres;

namespace Optepafi.Models.MapMan.MapInterfaces;

/// <summary>
/// Main interface for representing parsed map files.
/// 
/// Object implementing this interface can be then used for creation of map representations used in path finding algorithms and used for displaying maps for users.  
/// Main difference between implementations of this interface and implementations of <see cref="IMapRepre"/> is their computational complexity and their informative value.  
/// Map representations represents relations between map objects whereas map serves only as swiftly made collection of map objects stored for their further processing.  
/// Each implementation of this interface should have its own representative so it could presented as viable type in <see cref="MapManager"/> class.  
/// 
/// It provides modification of visitor pattern, so-called "generic visitor pattern".  
/// The main goal is not ensuring that caller implements specific method for every IMap implementation, but for ability to retrieve maps real type in form of type parameter.  
/// </summary>
public interface IMap
{
    /// <summary>
    /// Name of the source file represented by this instance.
    /// </summary>
    string FileName { get; init; }
    
    /// <summary>
    /// Path to the source file represented by this instance.
    /// </summary>
    string FilePath { get; init; }
    
    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        // IMapGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams)
        // where TGenericParam : TConstraint;
        
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>IMap</c> interface.
    /// 
    /// For more information on this pattern see <see cref="IMapGenericVisitor{TOut,TOtherParams}"/>
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor ;).</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of return value carried through visitor pattern.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of the rest of parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut AcceptGeneric<TOut, TOtherParams>(
        IMapGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>IMap</c> interface.
    /// 
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TOtherParams}"/>.
    /// </summary>
    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor);
    
    // public void AcceptGeneric(IMapGenericVisitor genericVisitor);
}

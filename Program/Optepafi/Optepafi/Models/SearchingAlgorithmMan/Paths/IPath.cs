using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths;

/// <summary>
/// Represents path found by some path finding algorithm.
/// This path is returned from path finding execution and then used by some ModelView for aggregating <see cref="IPathReport"/> by <see cref="ReportManager"/>.
/// It contains two type parameters which can be used for saving and transferring of vertex/edge attributes used lately for reports aggregation.
/// 
/// For transferring of paths should be used its predecessor <see cref="IPath"/>. This predecessor should not be directly implemented. Its only implementation should be this interface.
///
/// This interface provides modification of visitor pattern, so-called "generic visitor pattern".
/// The main goal is not ensuring that caller implements specific method for every IPath implementation, but for ability to retrieve templates real type in form of type parameter.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used for saving and transferring of vertex attributes used lately for reports aggregation.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used for saving a transferring of edge attributes used lately for reports aggregation.</typeparam>
public interface IPath<out TVertexAttributes, out TEdgeAttributes> : IPath
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>IPath{TVertexAttributes, TEdgeAttributes}</c> interface.
    /// For more information on this pattern see <see cref="IPathGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor :).</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of return value carried through visitor pattern.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of the rest of parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
}

/// <summary>
/// Predecessor of <see cref="IPath{TVertexAttributes,TEdgeAttributes}"/> type used for convenient transfer of paths.
/// This interface should not be directly implemented.
/// For more information see <see cref="IPath{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public interface IPath; 
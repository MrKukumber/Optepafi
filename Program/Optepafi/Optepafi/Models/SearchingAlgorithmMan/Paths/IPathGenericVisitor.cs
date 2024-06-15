using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths;


/// <summary>
/// One of generic visitor interfaces for <see cref="IPath{TVertexAttributes,TEdgeAttributes}"/> implementations. It provides access to modified visitor pattern on paths, where only one generic method is required to be implemented.
/// It serves mainly for acquiring generic parameter, that represents real type of visited path.
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value by GenericVisit.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface IPathGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visting method to be implemented.
    /// </summary>
    /// <param name="path">Path, which accepted the visit.</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TPath">Type of accepting path. Main result of this modified visitor pattern.</typeparam>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes carried by accepting path.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes carried by accepting path.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    public TOut GenericVisit<TPath, TVertexAttributes, TEdgeAttributes>(TPath path, TOtherParams otherParams)
        where TPath : IPath<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;
}
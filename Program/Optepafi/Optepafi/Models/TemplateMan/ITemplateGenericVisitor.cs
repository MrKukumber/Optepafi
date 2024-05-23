using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan;


/// <summary>
/// One of generic visitor interfaces for <see cref="ITemplate "/> implementations. It provides access to modified visitor pattern
/// on templates, where only one generic method is required to be implemented. It serves mainly for requiring generic
/// parameter, that represents real type of visited template. It is worth noted, that returning type is constraint
/// to be of type <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/> so that vertex attribute types a nd edge
/// attribute types defined by templates could be retrieved as well.
/// It has 3 other overloads for convenience of use. 
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of GenericVisit.</typeparam>
/// <typeparam name="TConstraint">Specifies constraint of TGenericParam type parameter.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface ITemplateGenericVisitor<TOut, TConstraint, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="template">Template, which accepted the visit.</param>
    /// <param name="genericParam">One generic parameter carried trough visitor pattern.</param>
    /// <param name="otherParams">Other parameters carried trough visitor pattern.</param>
    /// <typeparam name="TTemplate">Type of accepting template. Main result of this visit pattern modification.</typeparam>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes carried by accepting template.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes carried by accepting template.</typeparam>
    /// <typeparam name="TGenericParam">Type of generic parameter carried trough visitor pattern.</typeparam>
    /// <returns>Value of type TOut.</returns>
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TGenericParam>(TTemplate template, TGenericParam genericParam,
        TOtherParams otherParams) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
        where TGenericParam : TConstraint;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="ITemplate "/> implementations. It provides access to modified visitor pattern
/// on templates, where only one generic method is required to be implemented. It serves mainly for requiring generic
/// parameter, that represents real type of visited template. It is important to be noted, that returning type is constraint
/// to be of type <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/> so that vertex attribute types a nd edge
/// attribute types defined by templates could be retrieved as well.  
/// It has 3 other overloads for convenience of use. 
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of GenericVisit.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface ITemplateGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information of this method see <see cref="ITemplateGenericVisitor{TOut,TConstraint,TOtherParams}"/>.
    /// </summary>
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        TOtherParams otherParams)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="ITemplate "/> implementations. It provides access to modified visitor pattern
/// on templates, where only one generic method is required to be implemented. It serves mainly for requiring generic
/// parameter, that represents real type of visited template. It is important to be noted, that returning type is constraint
/// to be of type <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/> so that vertex attribute types a nd edge
/// attribute types defined by templates could be retrieved as well. 
/// It has 3 other overloads for convenience of use. 
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of GenericVisit.</typeparam>
public interface ITemplateGenericVisitor<TOut>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information of this method see <see cref="ITemplateGenericVisitor{TOut,TConstraint,TOtherParams}"/>.
    /// </summary>
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="ITemplate "/> implementations. It provides access to modified visitor pattern
/// on templates, where only one generic method is required to be implemented. It serves mainly for requiring generic
/// parameter, that represents real type of visited template. It is important to be noted, that returning type is constraint
/// to be of type <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/> so that vertex attribute types and edge
/// attribute types defined by templates could be retrieved as well. 
/// It has 3 other overloads for convenience of use. 
/// </summary>
public interface ITemplateGenericVisitor
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information on this method see <see cref="ITemplateGenericVisitor{TOut,TConstraint,TOtherParams}"/>.
    /// </summary>
    public void GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
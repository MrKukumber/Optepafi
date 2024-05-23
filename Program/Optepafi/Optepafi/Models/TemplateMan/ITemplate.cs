using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan;



/// <summary>
/// Represents templates of attributes used in graph's verteces and edges for describing their position and properties.
/// It represents not so much a contract as a identifier declaring what vertex and edge attributes are used in its name.
/// 
/// Its predecessor <see cref="ITemplate"/> should not be directly implemented. This interface should be its only implementation.
/// Actually, it is impossible correctly implement ITemplate interface, because of its AcceptGeneric methods that are part of visitor pattern on this type. They return template as visited type that is constrained by this interface, so template must implement this interface in order to correctly implement visitor pattern.
///
/// As mentioned above, it provides modification of visitor pattern, so-called "generic visitor pattern".
/// The main goal is not ensuring that caller implements method for every ITemplate implementation, but for ability to retrieve templates real type as in form of type parameter.
/// Methods of this pattern are already declared in its predecessor ITemplate, so they could be called right on ITemplate variables.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in the name of this template.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in the name of this template.</typeparam>
public interface ITemplate<TVertexAttributes,TEdgeAttributes> : ITemplate
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes 
{ }

/// <summary>
/// Predecessor of <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/> type used for more convenient transfer of templates.
/// This interface should not be directly implemented.
/// For more information see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public interface ITemplate
{
    string TemplateName { get; }

    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ITemplate</c> interface.
    /// For more information on this pattern see <see cref="ITemplateGenericVisitor"/>
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor :).</param>
    /// <param name="genericParam">One generic parameter carried trough visitor pattern.</param>
    /// <param name="otherParams">Other parameters carried trough visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of returned value of GenericVisit.</typeparam>
    /// <typeparam name="TGenericParam">Type of generic parameter carried trough visitor pattern.</typeparam>
    /// <typeparam name="TConstraint">Specifies constraint of TGenericParam type parameter.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type TOut.</returns>
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ITemplate</c> interface.
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TGenericParam,TConstraint,TOtherParams}"/>
    /// </summary>
    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ITemplate</c> interface.
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TGenericParam,TConstraint,TOtherParams}"/>
    /// </summary>
    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor);
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ITemplate</c> interface.
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TGenericParam,TConstraint,TOtherParams}"/>
    /// </summary>
    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor);
}

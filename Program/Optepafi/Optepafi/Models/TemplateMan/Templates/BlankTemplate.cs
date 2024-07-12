using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan.Templates;

/// <summary>
/// Template that contains blank vertex and blank edge attributes definitions. These attributes define no information to be beard.
/// This type is just demonstrative template for presenting application functionality.
/// For more information on templates see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public class BlankTemplate : ITemplate<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankTemplate Instance { get; } = new();
    private BlankTemplate(){}

    ///<inheritdoc cref="ITemplate.TemplateName"/>
    public string TemplateName { get; } = "Blank template";

    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut, TGenericParam, TConstraint, TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes, TGenericParam>(this, genericParam, otherParams);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut, TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this, otherParams);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric"/>
    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    /// <summary>
    /// Blank vertex attributes. They does not contain any information.
    /// This type are just demonstrative vertex attributes for presenting application functionality.
    /// For more information on templates see <see cref="IVertexAttributes"/>.
    /// </summary>
    public class VertexAttributes : IVertexAttributes
    {
        
    }

    /// <summary>
    /// Blank edge attributes. They does not contain any information.
    /// This type are just demonstrative edge attributes for presenting application functionality.
    /// For more information on templates see <see cref="IEdgeAttributes"/>.
    /// </summary>
    public class EdgeAttributes : IEdgeAttributes
    {
        
    }
}
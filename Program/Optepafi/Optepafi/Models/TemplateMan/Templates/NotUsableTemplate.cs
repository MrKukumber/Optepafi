using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan.Templates;

/// <summary>
/// Template that contains not usable vertex and blank edge attributes definitions. These attributes can not e used in any map representation.
/// This type is just demonstrative template for presenting application functionality.
/// For more information on templates see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public class NotUsableTemplate : ITemplate<NotUsableTemplate.VertexAttributes, NotUsableTemplate.EdgeAttributes>
{
    
    public static NotUsableTemplate Instance { get; } = new();
    private NotUsableTemplate(){}
    
    ///<inheritdoc cref="ITemplate.TemplateName"/>
    public string TemplateName { get; } = "Not usable template";
    
    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut, TGenericParam, TConstraint, TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint
    {
        return genericVisitor.GenericVisit<NotUsableTemplate, VertexAttributes, EdgeAttributes, TGenericParam>(this, genericParam, otherParams);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut, TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<NotUsableTemplate, VertexAttributes, EdgeAttributes>(this, otherParams);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<NotUsableTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    ///<inheritdoc cref="ITemplate.AcceptGeneric"/>
    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<NotUsableTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    /// <summary>
    /// Not usable vertex attributes. They are not usable in any map representation.
    /// This type are just demonstrative vertex attributes for presenting application functionality.
    /// For more information on templates see <see cref="IVertexAttributes"/>.
    /// </summary>
    public class VertexAttributes : IVertexAttributes;

    /// <summary>
    /// Not usable edge attributes. They are not usable in any map representation.
    /// This type are just demonstrative vertex attributes for presenting application functionality.
    /// For more information on templates see <see cref="IVertexAttributes"/>.
    /// </summary>
    public class EdgeAttributes : IEdgeAttributes;

}
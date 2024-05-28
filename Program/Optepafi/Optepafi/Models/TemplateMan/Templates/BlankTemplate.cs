using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan.Templates;

public class BlankTemplate : ITemplate<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankTemplate Instance { get; } = new();
    private BlankTemplate(){}

    public string TemplateName { get; } = "Blank template";

    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes, TGenericParam>(this, genericParam, otherParams);
    }

    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this, otherParams);
    }

    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<BlankTemplate, VertexAttributes, EdgeAttributes>(this);
    }

    public class VertexAttributes : IVertexAttributes { }

    public class EdgeAttributes : IEdgeAttributes { }
}
using System.Dynamic;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan.Templates;

public class Orienteering_ISOM_2017_2 : ITemplate<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static Orienteering_ISOM_2017_2 Instance { get; } = new();
    private Orienteering_ISOM_2017_2() {}
    public string TemplateName { get; } = "Orienteering (ISOM 2017-2)";

    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint
    {
        return genericVisitor.GenericVisit<Orienteering_ISOM_2017_2, VertexAttributes, EdgeAttributes, TGenericParam>(
            this, genericParam, otherParams);
    }
    
    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams)
    {
        return  genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes, EdgeAttributes>(this, otherParams);
    }

    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes,EdgeAttributes>(this);
    }

    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes,EdgeAttributes>(this);
    }

    //TODO: implement
    public class VertexAttributes : IVertexAttributes
    {
        
    }

    public class EdgeAttributes : IEdgeAttributes
    {
        
    }
}
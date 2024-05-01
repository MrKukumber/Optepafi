using System.Dynamic;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan.Templates;

public class Orienteering_ISOM_2017_2 : ITemplate<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static Orienteering_ISOM_2017_2 Instance { get; } = new();
    private Orienteering_ISOM_2017_2() {}
    public string TemplateName { get; } = "Orienteering (ISOM 2017-2)";

    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams)
    {
        return  genericVisitor.GenericVisit(this, otherParams);
    }

    public class VertexAttributes : IVertexAttributes
    {
        
    }

    public class EdgeAttributes : IEdgeAttributes
    {
        
    }
}
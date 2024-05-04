using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreRepres;

public class FunctionalObjectRepreRep<TTemplate, TVertexAttributes, TEdgeAttributes> :
    IDefinedFunctionalityMapRepreRepresentativ<IFunctionalObjectRepre<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static FunctionalObjectRepreRep<TTemplate, TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private FunctionalObjectRepreRep() { }
}
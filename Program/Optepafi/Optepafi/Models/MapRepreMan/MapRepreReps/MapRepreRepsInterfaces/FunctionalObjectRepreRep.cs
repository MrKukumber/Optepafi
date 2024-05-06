using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreReps.MapRepreRepsInterfaces;

public class FunctionalObjectRepreRep<TVertexAttributes, TEdgeAttributes> :
    IDefinedFunctionalityMapRepreRepresentativ<IFunctionalObjectRepre<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static FunctionalObjectRepreRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private FunctionalObjectRepreRep() { }
}
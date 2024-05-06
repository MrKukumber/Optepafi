using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepresentationGenericVisitor<out TOut,in TConstraint,in TOtherParams>
{
    public TOut GenericVisit<TMapRepre, TVertexAttributes, TEdgeAttributes, TGenericParam>(TMapRepre mapRepre, TGenericParam genericParam,
        TOtherParams otherParams)
        where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
        where TGenericParam : TConstraint;
}
public interface IMapRepresentationGenericVisitor<out TOut, in TOtherParams>
{
    public TOut GenericVisit<TMapRepre, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre, TOtherParams otherParams)
        where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
public interface IMapRepresentationGenericVisitor<out TOut>
{
    public TOut GenericVisit<TMapRepre, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre)
        where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
public interface IMapRepresentationGenericVisitor
{
    public void GenericVisit<TMapRepre, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre)
        where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

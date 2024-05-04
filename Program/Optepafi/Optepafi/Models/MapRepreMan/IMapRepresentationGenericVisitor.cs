using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepresentationGenericVisitor<out TOut,in TConstraint,in TOtherParams>
{
    public TOut GenericVisit<TMapRepre, TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes, TGenericParam>(TMapRepre mapRepre, TGenericParam genericParam,
        TOtherParams otherParams)
        where TMapRepre : IMapRepreWithDefinedFunctionality<TTemplate, TVertexAttributes, TEdgeAttributes>
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertex : IVertex
        where TEdge : IEdge
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
        where TGenericParam : TConstraint;
}
public interface IMapRepresentationGenericVisitor<out TOut, in TOtherParams>
{
    public TOut GenericVisit<TMapRepre, TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre, TOtherParams otherParams)
        where TMapRepre : IMapRepreWithDefinedFunctionality<TTemplate, TVertexAttributes, TEdgeAttributes>
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertex : IVertex
        where TEdge : IEdge
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
public interface IMapRepresentationGenericVisitor<out TOut>
{
    public TOut GenericVisit<TMapRepre, TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre)
        where TMapRepre : IMapRepreFunctionality<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertex : IVertex
        where TEdge : IEdge
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
public interface IMapRepresentationGenericVisitor
{
    public void GenericVisit<TMapRepre, TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(
        TMapRepre mapRepre)
        where TMapRepre : IMapRepreFunctionality<TTemplate, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertex : IVertex
        where TEdge : IEdge
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Verteces;

public interface IBasicEdgeCoupledBasicVertex<TVertexAttributes, TEdgeAttributes> :
    IBasicVertex<IBasicEdge<IBasicEdgeCoupledBasicVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

public interface IBasicVertex<TEdge, out TVertexAttributes> :  
    IEdgesContainingVertex<TEdge>,
    IAttributeBearingVertex<TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdge : IEdge
{
    
}

public interface IEdgesContainingVertex<TEdge> : IVertex
    where TEdge : IEdge
{
    TEdge[] GetEdges();
    void SetWeight(int weight, TEdge edge);
    int? GetWeight(TEdge edge);
}

public interface IAttributeBearingVertex<out TVertexAttributes> : IVertex
    where TVertexAttributes : IVertexAttributes
{
    TVertexAttributes Attributes { get; }
}

public interface IVertex 
{
    
}
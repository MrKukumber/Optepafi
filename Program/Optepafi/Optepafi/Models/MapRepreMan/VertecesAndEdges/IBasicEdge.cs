using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.VertecesAndEdges;

public interface IBasicEdge<out TVertex, out TEdgeAttributes> : 
    IDestVertexContainingEdge<TVertex>, 
    IAttributesBearingEdge<TEdgeAttributes>
    where TEdgeAttributes : IEdgeAttributes
    where TVertex : IVertex
{
    
}

public interface IDestVertexContainingEdge<out TVertex> : IEdge
    where TVertex : IVertex
{
    TVertex To { get; }
}

public interface IAttributesBearingEdge<out TEdgeAttributes> : IEdge
    where TEdgeAttributes : IEdgeAttributes
{
    TEdgeAttributes Attributes { get; }
}

public interface IEdge
{
    
}




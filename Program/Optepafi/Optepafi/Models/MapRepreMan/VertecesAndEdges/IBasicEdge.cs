using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Verteces;

public class BasicEdge<TEdgeAttributes, TVertexAttributes> :
    IBasicEdge<BasicVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>
    where TEdgeAttributes : IEdgeAttributes
    where TVertexAttributes : IVertexAttributes
{
    
}
public interface IBasicEdge<out TBasicVertex, out TEdgeAttributes> : 
    IDestVertexContainingEdge<TBasicVertex>, 
    IAttributesBearingEdge<TEdgeAttributes>
    where TEdgeAttributes : IEdgeAttributes
    where TBasicVertex : IVertex
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




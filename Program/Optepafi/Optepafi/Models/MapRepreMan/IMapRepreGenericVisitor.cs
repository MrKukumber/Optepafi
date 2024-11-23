using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan;
//TODO: comment

// public interface IMapRepreGenericVisitor<TOut, TConstraint, TOtherParams>
// {
    // public TOut GenericVisit<TImplementation, TVertexAttributes, TEdgeAttributes, TGenericParam>(TImplementation implementation, TGenericParam genericParam,
        // TOtherParams otherParams) 
        // where TImplementation : IGraph<TVertexAttributes, TEdgeAttributes>
        // where TVertexAttributes : IVertexAttributes
        // where TEdgeAttributes : IEdgeAttributes
        // where TGenericParam : TConstraint;
// }

public interface IMapRepreGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TImplementation, TVertexAttributes, TEdgeAttributes>(TImplementation implementation,
        TOtherParams otherParams)
        where TImplementation : IGraph<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

// public interface IMapRepreGenericVisitor<TOut>
// {
    // public TOut GenericVisit<TImplementation, TVertexAttributes, TEdgeAttributes>(TImplementation implementation) 
        // where TImplementation : IGraph<TVertexAttributes, TEdgeAttributes>
        // where TVertexAttributes : IVertexAttributes
        // where TEdgeAttributes : IEdgeAttributes;
// }

// public interface IMapRepreGenericVisitor
// {
    // public void GenericVisit<TImplementation, TVertexAttributes, TEdgeAttributes>(TImplementation implementation)
        // where TImplementation : IGraph<TVertexAttributes, TEdgeAttributes>
        // where TVertexAttributes : IVertexAttributes
        // where TEdgeAttributes : IEdgeAttributes;
// }

using System.Collections.Generic;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

/// <summary>
/// Blank graph. It will contain no objects representing provided map, nor implement any functionality at all.
/// 
/// It is just blank object used as demonstrative type for presenting application functionality.  
/// For more information about graphs see <see cref="IGraph{TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
public interface IBlankGraph :
    IBlankRepre,
    IGraph<IBlankGraph.Vertex<IVertexAttributes>, IBlankGraph.Edge<IEdgeAttributes>>
{
    public class Vertex<TVertexAttributes>(TVertexAttributes vertexAttributes) : IAttributeBearingVertex<TVertexAttributes>
        where TVertexAttributes : IVertexAttributes
    {
        public TVertexAttributes Attributes { get; } = vertexAttributes;
    }

    public class Edge<TEdgeAttributes>(TEdgeAttributes edgeAttributes) : IAttributesBearingEdge<TEdgeAttributes>
        where TEdgeAttributes : IEdgeAttributes
    {
        
        public TEdgeAttributes Attributes { get; } = edgeAttributes;
    }
}
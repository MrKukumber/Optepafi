using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

//TODO: comment + add functionalities
public interface ICompleteSnappingGraph: 
    ICompleteSnappingMapRepre,
    IPredecessorRemembering<ICompleteSnappingGraph.Vertex<IVertexAttributes, IEdgeAttributes>, ICompleteSnappingGraph.Edge<IEdgeAttributes, IVertexAttributes>>
{


    public class Vertex<TVertexAttributes, TEdgeAttributes>(TVertexAttributes attributes, IEnumerable<Edge<TEdgeAttributes, TVertexAttributes>> outgoingEdges) : 
        IBasicVertex<Edge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes>,
        IPredecessorRememberingVertex
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        protected Dictionary<Edge<TEdgeAttributes, TVertexAttributes>, int?> _outgoingWeightedEdges = outgoingEdges.ToDictionary( edge => edge, _ => (int?) null);
        protected TVertexAttributes _attributes = attributes;
        
        public TVertexAttributes Attributes => _attributes;
        
        public Edge<TEdgeAttributes, TVertexAttributes>[] GetEdges() => _outgoingWeightedEdges.Keys.ToArray();
        public void SetWeight(int? weight, Edge<TEdgeAttributes, TVertexAttributes> edge) => _outgoingWeightedEdges[edge] = weight;
        public int? GetWeight(Edge<TEdgeAttributes, TVertexAttributes> edge) => _outgoingWeightedEdges[edge];

        public IPredecessorRememberingVertex? Predecessor { get; set; }
    }

    public struct Edge<TEdgeAttributes, TVertexAttributes>(TEdgeAttributes attributes, Vertex<TVertexAttributes, TEdgeAttributes> destination) : 
        IBasicEdge<Vertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>
        where TEdgeAttributes : IEdgeAttributes
        where TVertexAttributes : IVertexAttributes
    {
        public TEdgeAttributes Attributes { get; } = attributes;
        
        public Vertex<TVertexAttributes, TEdgeAttributes> To { get; } = destination;
    }
}
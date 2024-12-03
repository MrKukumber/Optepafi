using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

//TODO: comment + add functionalities
public interface ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>: 
    ICompleteNetIntertwiningMapRepre,
    IPredecessorRemembering<ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Edge>,
    IScaled<ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Edge> 
    where TVertexAttributes: IVertexAttributes
    where TEdgeAttributes: IEdgeAttributes
{


    public class Vertex(TVertexAttributes attributes, IEnumerable<Edge> outgoingEdges) : 
        IBasicVertex<Edge, TVertexAttributes>,
        IPredecessorRememberingVertex
    {
        protected Dictionary<Edge, int?> _outgoingWeightedEdges = outgoingEdges.ToDictionary( edge => edge, _ => (int?) null);
        protected TVertexAttributes _attributes = attributes;
        
        public TVertexAttributes Attributes => _attributes;
        
        public Edge[] GetEdges() => _outgoingWeightedEdges.Keys.ToArray();
        public void SetWeight(int? weight, Edge edge) => _outgoingWeightedEdges[edge] = weight;
        public int? GetWeight(Edge edge) => _outgoingWeightedEdges[edge];

        public IPredecessorRememberingVertex? Predecessor { get; set; }
    }

    public struct Edge(TEdgeAttributes attributes, Vertex destination) : 
        IBasicEdge<Vertex, TEdgeAttributes>
    {
        public TEdgeAttributes Attributes { get; } = attributes;
        
        public Vertex To { get; } = destination;
    }
}
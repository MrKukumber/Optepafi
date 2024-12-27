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
    IPredecessorRemembering<TVertexAttributes, TEdgeAttributes>,
    IScaled<ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Edge> 
    where TVertexAttributes: IVertexAttributes
    where TEdgeAttributes: IEdgeAttributes
{


    public class Vertex(TVertexAttributes attributes, IEnumerable<IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>> outgoingEdges) : 
        IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>
    {
        protected Dictionary<IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>, int?> _outgoingWeightedEdges = outgoingEdges.ToDictionary( edge => edge, _ => (int?) null);
        protected TVertexAttributes _attributes = attributes;
        
        public TVertexAttributes Attributes => _attributes;
        public IEnumerable<IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>> GetEdges() => _outgoingWeightedEdges.Keys;
        public void SetWeight(int? weight, IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes> edge) => _outgoingWeightedEdges[edge] = weight;
        public int? GetWeight(IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes> edge) => _outgoingWeightedEdges[edge];

        public IPredecessorRememberingVertex? Predecessor { get; set; }
    }

    public class Edge(TEdgeAttributes attributes, Vertex destination) : 
        IPredecessorRememberingVertexCoupledBasicEdge<TEdgeAttributes, TVertexAttributes>
    {
        public TEdgeAttributes Attributes { get; } = attributes;
        
        public IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes> To { get; } = destination;
    }
}
using System.Collections.Generic;
using System.Linq;
using Avalonia.Animation;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;

//TODO: comment + add functionalities
public interface ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>: 
    ICompleteNetIntertwiningMapRepre,
    IPredecessorRemembering<ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Edge, float, TVertexAttributes, TEdgeAttributes>,
    IScaled<ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Edge>
    where TVertexAttributes: IVertexAttributes
    where TEdgeAttributes: IEdgeAttributes
{


    public class Vertex(TVertexAttributes attributes, IEnumerable<Edge> outgoingEdges) : 
        IBasicVertex<Edge, TVertexAttributes>,
        IPredecessorRememberingVertex<TVertexAttributes>
    {
        protected Dictionary<Edge, float> _outgoingWeightedEdges = outgoingEdges.ToDictionary( edge => edge, _ => float.NaN);
        protected TVertexAttributes _attributes = attributes;
        
        public TVertexAttributes Attributes => _attributes;
        public IEnumerable<Edge> GetEdges() => _outgoingWeightedEdges.Keys;
        // public void SetWeight(float weight, Edge edge) => _outgoingWeightedEdges[edge] = weight;

        // public bool TryGetWeight(Edge edge, out float weight)
        // {
            // if (!_outgoingWeightedEdges.ContainsKey(edge))
            // {
                // weight = float.NaN;
                // return false;
            // }
            // weight = _outgoingWeightedEdges[edge];
            // return _outgoingWeightedEdges[edge] is not float.NaN;
        // }
        public IPredecessorRememberingVertex<TVertexAttributes>? Predecessor { get; set; }
    }

    // TODO: podivne chovanie, ked to bola struktura - ked som chcel priradit CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation
    // do Graph<IVertex, IEdge> tak to neslo prave preto, ze Edge bola struktura
    public class Edge(TEdgeAttributes attributes, Vertex destination) : 
        IBasicEdge<Vertex, TEdgeAttributes, float>
    {
        /// initially all weights are set to float.NaN
        protected float _weight = float.NaN;
        public void SetWeight(float weight) => _weight = weight;
        public float GetWeight() => _weight;
        public TEdgeAttributes Attributes { get; } = attributes;
        public Vertex To { get; } = destination;
    }
}
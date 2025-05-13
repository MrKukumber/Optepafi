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


    public class Vertex : 
        IBasicVertex<Edge, TVertexAttributes>,
        IPredecessorRememberingVertex<TVertexAttributes>
    {
        public Vertex() { }
        public Vertex(TVertexAttributes attributes, IEnumerable<Edge> outgoingEdges)
        {
            _outgoingEdges = outgoingEdges;
            _attributes = attributes;
        }
        
        protected IEnumerable<Edge> _outgoingEdges;
        protected TVertexAttributes _attributes;
        
        public TVertexAttributes Attributes => _attributes;
        public IEnumerable<Edge> GetEdges() => _outgoingEdges;
        public IPredecessorRememberingVertex<TVertexAttributes>? Predecessor { get; set; }
    }

    // TODO: podivne chovanie, ked to bola struktura - ked som chcel priradit CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation
    // do Graph<IVertex, IEdge> tak to neslo prave preto, ze Edge bola struktura
    public class Edge : 
        IBasicEdge<Vertex, TEdgeAttributes, float>
    {
        public Edge(){}
        public Edge(TEdgeAttributes attributes, Vertex destination)
        {
            _to  = destination;
            _attributes = attributes;
        }
        protected Vertex _to;
        protected TEdgeAttributes _attributes;
        /// initially all weights are set to float.NaN
        protected float _weight = float.NaN;
        
        public void SetWeight(float weight) => _weight = weight;
        public float GetWeight() => _weight;
        public TEdgeAttributes Attributes => _attributes;
        public Vertex To => _to;
    }
}
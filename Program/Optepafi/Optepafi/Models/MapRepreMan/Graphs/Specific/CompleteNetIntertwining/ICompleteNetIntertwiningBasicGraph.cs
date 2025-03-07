using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.Graphs.Specific.Blank;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;

public interface ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttibutes> :
    ICompleteNetIntertwiningMapRepre,
    ISearchable<ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttibutes>.Vertex, ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttibutes>.Edge, float, TVertexAttributes, TEdgeAttibutes>,
    IScaled<ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttibutes>.Vertex, ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttibutes>.Edge>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttibutes : IEdgeAttributes
{
    public class Vertex(TVertexAttributes attributes, IEnumerable<Edge> outgoingEdges) :
        IBasicVertex<Edge, TVertexAttributes>
    {
        protected HashSet<Edge> _outgoingEdges = outgoingEdges.ToHashSet();
        protected TVertexAttributes _attributes = attributes;
        
        public TVertexAttributes Attributes => _attributes;
        public IEnumerable<Edge> GetEdges() => _outgoingEdges;
    }

    public class Edge(TEdgeAttibutes attributes, Vertex destination) :
        IBasicEdge<Vertex, TEdgeAttibutes, float>
    {
        protected float _weight = float.NaN;
        public void SetWeight(float weight) => _weight = weight;
        public float GetWeight() => _weight;
        public TEdgeAttibutes Attributes { get; } = attributes;
        public Vertex To { get; } = destination;
    }
}
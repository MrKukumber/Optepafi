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


    public class Vertex<TVertexAttributes, TEdgeAttributes> : 
        IBasicVertex<Edge<TEdgeAttributes, TVertexAttributes>, TVertexAttributes>,
        IPredecessorRememberingVertex
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        
        public TVertexAttributes Attributes { get; }
        
        public Edge<TEdgeAttributes, TVertexAttributes>[] GetEdges()
        {
            throw new System.NotImplementedException();
        }

        public void SetWeight(int weight, Edge<TEdgeAttributes, TVertexAttributes> edge)
        {
            throw new System.NotImplementedException();
        }

        public int? GetWeight(Edge<TEdgeAttributes, TVertexAttributes> edge)
        {
            throw new System.NotImplementedException();
        }

        public IPredecessorRememberingVertex? Predecessor { get; set; }
    }

    public struct Edge<TEdgeAttributes, TVertexAttributes> : 
        IBasicEdge<Vertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>
        where TEdgeAttributes : IEdgeAttributes
        where TVertexAttributes : IVertexAttributes
    {
        public TEdgeAttributes Attributes { get; }
        
        public Vertex<TVertexAttributes, TEdgeAttributes> To { get; }
    }
}
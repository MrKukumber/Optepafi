using Optepafi.Models.MapRepreMan.Graphs.Functionalities;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

//TODO: comment + add functionalities
public interface ICompleteSnappingGraph<TVertexAttributes, TEdgeAttributes> : 
    ICompleteSnappingMapRepre,
    IGraph<TVertexAttributes, TEdgeAttributes>,
    IPredecessorRemembering< ICompleteSnappingGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteSnappingGraph<TVertexAttributes, TEdgeAttributes>.Edge, TVertexAttributes , TEdgeAttributes> 
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{


    public class Vertex : IBasicVertex<Edge, TVertexAttributes>,
        IPredecessorRememberingVertex<Edge, TVertexAttributes>
    {
        
        public Edge[] GetEdges()
        {
            throw new System.NotImplementedException();
        }

        public void SetWeight(int weight, Edge edge)
        {
            throw new System.NotImplementedException();
        }

        public int? GetWeight(Edge edge)
        {
            throw new System.NotImplementedException();
        }

        public TVertexAttributes Attributes { get; }
        public IPredecessorRememberingVertex<Edge, TVertexAttributes>? Predecessor { get; set; }
    }

    public struct Edge : IBasicEdge<Vertex, TEdgeAttributes>
    {
        
    }
}
using Optepafi.Models.MapRepreMan.Graphs.GraphFunctionalities;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;



public interface IObjectRepreGraph<TVertexAttributes, TEdgeAttributes> :
    IObjectRepre,
    IGraph<TVertexAttributes, TEdgeAttributes>,
    IPredecessorRemembering<
        IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, 
        IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>, 
        TVertexAttributes, TEdgeAttributes>,
    IAStarHeuristicEnsuring<IBasicEdgeCoupledBasicVertex<TVertexAttributes, TEdgeAttributes>, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}

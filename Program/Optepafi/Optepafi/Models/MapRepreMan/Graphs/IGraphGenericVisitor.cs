using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs;

//TODO: comment
public interface IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams>
{
    public TOut GenericVisit<TGraph, TVertex, TEdge, TGenericParam1, TGenericParam2>(TGraph graph,  TOtherParams otherParams)
        where TGraph : IGraph<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge
        where TGenericParam1 : TGenericConstraint1
        where TGenericParam2 : TGenericConstraint2;
}
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;

public interface IAStarHeuristicEnsuring<in TVertex, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TVertex : IAttributeBearingVertex<TVertexAttributes>
{
    int GetWeightFromHeuristic(TVertex from, TVertex to);
}
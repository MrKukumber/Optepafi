using Optepafi.Models.MapRepreMan.Verteces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IAStarHeuristicEnsuring<in TVertex, TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TVertex : IAttributeBearingVertex<TVertexAttributes>
{
    int GetWeightFromHeuristic(TVertex from, TVertex to);
}
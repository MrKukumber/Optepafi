using Optepafi.Models.MapRepreMan.VertecesAndEdges;

namespace Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;

public interface IInstantWholeGraphDelivering<out TVertex>
    where TVertex : IVertex
{
    TVertex[] GetWholeGraph();
}
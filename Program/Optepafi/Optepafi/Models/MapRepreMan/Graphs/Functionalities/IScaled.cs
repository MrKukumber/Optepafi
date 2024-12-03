using Optepafi.Models.MapRepreMan.VertecesAndEdges;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

public interface IScaled<out TVertex, out TEdge> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
{
    int Scale { get; }
}
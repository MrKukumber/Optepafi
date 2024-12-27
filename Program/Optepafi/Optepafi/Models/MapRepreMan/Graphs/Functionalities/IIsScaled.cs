using Optepafi.Models.MapRepreMan.VertecesAndEdges;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

//TODO: comment
/// <summary>
/// This interface should not be implemented. Its successor <see cref="IScaled{TVertex,TEdge}"/> should be implemented instead.
/// </summary>
public interface IIsScaled<out TVertex, out TEdge> : IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge 
{ }

//TODO: comment
public interface IScaled<out TVertex, out TEdge> : IIsScaled<TVertex, TEdge>, IGraph<TVertex, TEdge>
    where TVertex : IVertex
    where TEdge : IEdge
{
    int Scale { get; }
}
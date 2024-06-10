using Optepafi.Models.SearchingAlgorithmMan.Paths;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

public interface IPathGraphicsAggregator<in TPath> : IGraphicsAggregator
    where TPath : IPath
{
    public void AggregateGraphics(TPath path, IGraphicsObjectCollector collectorForAggregatedObjects);
}
using System.Threading;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.ModelViews.Graphics;
using Optepafi.ModelViews.Graphics.Collectors;

namespace Optepafi.Models.Graphics;

public interface IPathGraphicsAggregator<in TPath> : IGraphicsAggregator
    where TPath : IPath
{
    public void AggregateGraphics(TPath path, IGraphicsObjectCollector collectorForAggregatedObjects);
}
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.ModelViews.Graphics;
using Optepafi.ModelViews.Graphics.Collectors;

namespace Optepafi.Models.Graphics;

public interface IMapGraphicsAggregator<in TMap> : IGraphicsAggregator
    where TMap : IMap
{
    public void AggregateGraphics(TMap map, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);

    public (int minXPos, int minYPos, int maxXPos, int maxYPos) GetAxisExtremesOf(TMap map);
}
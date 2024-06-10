using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

public interface IMapGraphicsAggregator<in TMap> : IGraphicsAggregator
    where TMap : IMap
{
    public void AggregateGraphics(TMap map, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);

    public (int minXPos, int minYPos, int maxXPos, int maxYPos) GetAxisExtremesOf(TMap map);
}
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.MapRepreMan.MapRepres;

namespace Optepafi.Models.GraphicsMan.Aggregators;

//TODO: comment
public interface IMapRepreGraphicsAggregator<in TMapRepre> : IGraphicsAggregator
    where TMapRepre : IMapRepre
{
    public void AggregateGraphics(TMapRepre map, IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
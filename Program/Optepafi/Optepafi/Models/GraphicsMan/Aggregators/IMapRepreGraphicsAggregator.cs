using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.MapRepreMan.MapRepres;

namespace Optepafi.Models.GraphicsMan.Aggregators;

//TODO: comment
public interface IMapRepreGraphicsAggregator<in TImplementation> : IGraphicsAggregator
    where TImplementation : IMapRepre
{
    public void AggregateGraphics(TImplementation map, IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
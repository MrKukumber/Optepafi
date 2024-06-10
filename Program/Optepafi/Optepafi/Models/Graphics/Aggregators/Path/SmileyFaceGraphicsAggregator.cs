using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Implementations;

namespace Optepafi.Models.Graphics.GraphicsAggregators.Path;

public class SmileyFaceGraphicsAggregator : IPathGraphicsAggregator<SmileyFacePath>
{
    public void AggregateGraphics(SmileyFacePath path, IGraphicsObjectCollector collectorForAggregatedObjects)
    {
        collectorForAggregatedObjects.Add(new SmileyFaceObject(path.StartPosition));
        collectorForAggregatedObjects.Add(new SmileyFaceObject(path.EndPosition));
    }
}
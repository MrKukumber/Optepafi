using System.Threading;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.Graphics.GraphicsAggregators.Path;

public class SmileyFaceGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : IPathGraphicsAggregator<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFaceGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFaceGraphicsAggregator(){}
    public void AggregateGraphics(SmileyFacePath<TVertexAttributes, TEdgeAttributes> path, 
        IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel, 
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        collectorForAggregatedObjects.Add(new SmileyFaceObject(path.StartPosition));
        collectorForAggregatedObjects.Add(new SmileyFaceObject(path.EndPosition));
    }
}
using System.Threading;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

public interface IPathGraphicsAggregator<in TPath, TVertexAttributes, TEdgeAttributes> : IGraphicsAggregator
    where TPath : IPath<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public void AggregateGraphics(TPath path, IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
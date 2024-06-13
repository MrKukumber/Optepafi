using System.Threading;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

public interface ISearchingStateGraphicsAggregator<in TSearchingState, TVertexAttributes, TEdgeAttributes> : IGraphicsAggregator
    where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes> 
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public void AggregateGraphics(TSearchingState searchingState,
        IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.GraphicsMan.Aggregators;

/// <summary>
/// Represents aggregator of graphic objects for searching states of specific type.
/// 
/// Graphics is aggregated into provided collectors. This aggregation will probably run asynchronously, so aggregated objects could be used right after their submission to collector.  
/// Algorithms can not by default extract information from vertex and edge attributes. They are therefore able to provide searching states which contains only non processed vertex/edge attributes.  
/// For this reason searching state graphics aggregator will also receive corresponding user model, which may be able to extract necessary information from attributes contained in searching state.  
/// The word "may" is important one. No one will ensure, that user model is able to deliver some service. Aggregator should be prepared for user model not containing some functionality and simply not include information dependent on it in aggregated graphics.  
/// </summary>
/// <typeparam name="TSearchingState">Type of searching states for which graphics is aggregated.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which searching state can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which searching state can contain and user model can use for computing.</typeparam>
public interface ISearchingStateGraphicsAggregator<in TSearchingState, TVertexAttributes, TEdgeAttributes> : IGraphicsAggregator
    where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes> 
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Aggregates graphics of provided searching state into provided collector.
    /// 
    /// It can ask inserted user model for some information extraction service on vertex/edge attributes. It is not ensured that user model will provide such service. For more on this topic see <see cref="ISearchingStateGraphicsAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.  
    /// Aggregation will probably run asynchronously, so aggregated objects could be used right after their submission.  
    /// Because of that, objects should be submitted into collector immediately after their creation, so the app logic could process them.  
    /// </summary>
    /// <param name="searchingState">Searching state on which aggregation of graphic objects is based.</param>
    /// <param name="userModel">User model which can be asked for some information extraction.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling aggregation.</param>
    public void AggregateGraphics(TSearchingState searchingState,
        IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
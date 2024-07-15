using System.Threading;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.GraphicsMan.Aggregators;

/// <summary>
/// Represents aggregator of graphic objects for paths of specific type.
/// 
/// Graphics is aggregated into provided collectors. This aggregation will probably run asynchronously, so aggregated objects could be used right after their submission to collector.  
/// Algorithms can not by default extract information from vertex and edge attributes. They are therefore able to provide paths which contains only non processed vertex/edge attributes.  
/// For this reason path aggregator will receive corresponding user model, which may be able to extract necessary information from attributes contained in path.  
/// The word "may" is important one. No one will ensure, that user model is able to deliver some service. Aggregator should be prepared for user model not containing some functionality and simply not include information dependent on it in aggregated graphics.  
/// </summary>
/// <typeparam name="TPath">Type of path for which graphics is aggregated.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which path can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which path can contain and user model can use for computing.</typeparam>
public interface IPathGraphicsAggregator<in TPath, TVertexAttributes, TEdgeAttributes> : IGraphicsAggregator
    where TPath : IPath<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Aggregates graphics of provided path into provided collector.
    /// 
    /// It can ask inserted user model for some information extraction service on vertex/edge attributes. It is not ensured that user model will provide such service. For more on this topic see <see cref="IPathGraphicsAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.  
    /// Aggregation will probably run asynchronously, so aggregated objects could be used right after their submission.  
    /// Because of that, objects should be submitted into collector immediately after their creation, so the app logic could process them.  
    /// </summary>
    /// <param name="path">Path on which aggregation of graphic objects is based.</param>
    /// <param name="userModel">User model which can be asked for some information extraction.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling aggregation.</param>
    public void AggregateGraphics(TPath path, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
}
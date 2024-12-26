using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.GraphicsMan.Aggregators;
using Optepafi.Models.GraphicsMan.Aggregators.Path;
using Optepafi.Models.GraphicsMan.Aggregators.SearchingState;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.GraphicsMan;

/// <summary>
/// Singleton class representing sub-manager for graphics aggregation.
/// 
/// It is main channel between graphics aggregation mechanisms and other managers/constructs of Model.  
/// It provides supporting methods for correct calling of graphics aggregators.  
/// Its functionality is similar to <see cref="GraphicsManager"/> singelton class which is intended for use from applications logic.  
/// Main difference in way of use of these classes is that, this class has two generic parameters (representing used vertex and edge attributes types in aggregation of graphics) and in provided methods request for specific properties of inserted object types.  
/// This is the reason why this class is intended for use from Model constructs. It is property of these constructs they know functionalities of their data already . So the check for their functionality again would be redundant. They also work with specific type of vertex/edge attributes so the are able to provide them.  
/// 
/// For more information of process of graphics aggregation see <see cref="GraphicsManager"/>.  
/// In case of paths and searching states graphics there is one specialty though. In the process of aggregation there is usually need for user models support.  
/// Provided paths often bears vertex and edge attributes so it is necessary for user models to be able to process these attributes and provide usable values.  
/// User models should at least be able to retrieve positions from attributes and return them to aggregators, so they could construct the most basic graphics.  
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in aggregation.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in aggregation.</typeparam>
public class GraphicsSubManager<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static GraphicsSubManager<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private GraphicsSubManager(){}
    
    /// <summary>
    /// Collection of aggregators for specific path types. It is searched through when path graphics is to be aggregated.
    /// </summary>
    public IReadOnlySet<IGraphicsAggregator> PathGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(SegmentedLinePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes>.Instance, SmileyFacePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes>.Instance);
    
    /// <summary>
    /// Collection of aggregators for specific searching state types. It is searched when searching state is to be aggregated.
    /// </summary>
    public IReadOnlySet<IGraphicsAggregator> SearchingStateGraphicsAggregators { get; } =
        ImmutableHashSet.Create<IGraphicsAggregator>(SmileyFacePathDrawingStateGraphicsAggregator<TVertexAttributes, TEdgeAttributes>.Instance);
    
    public enum AggregationResult {Aggregated, NoUsableAggregatorFound}

    /// <summary>
    /// Method for aggregating of paths graphics.
    /// 
    /// It accepts path and collector which will be filled with aggregated objects.  
    /// It also requests user model, that can be used for computing of some values for aggregator. No specific functionality is forced upon provided user model. More about usage of user models in <see cref="IPathGraphicsAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.  
    /// It runs through <c>PathGraphicsAggregators</c> and looks for appropriate graphics aggregator by pattern-matching their generic parameter <c>TPath</c> with type of provided path.  
    /// When such aggregator is found, its aggregating method is called.  
    /// </summary>
    /// <param name="path">Path which graphics is to be aggregated.</param>
    /// <param name="userModel">User model that is used for aggregation of path graphics.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated path graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancellation of aggregation.</param>
    /// <typeparam name="TPath">Type of path which graphics is to be aggregated. It is used for finding appropriate aggregator.</typeparam>
    /// <returns>Result of aggregation</returns>
    public AggregationResult AggregatePathGraphics<TPath>(TPath path,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
        where TPath : IPath<TVertexAttributes, TEdgeAttributes>
    {
        foreach (var graphicsAggregator in PathGraphicsAggregators)
        {
            if (graphicsAggregator is IPathGraphicsAggregator<TPath, TVertexAttributes, TEdgeAttributes> pathGraphcisAggregator)
            {
                pathGraphcisAggregator.AggregateGraphics(path, userModel, collectorForAggregatedObjects, cancellationToken);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }

    /// <summary>
    /// Method for aggregating of searching states graphics.
    /// 
    /// It accepts searching state and collector which will be filled with aggregated objects.  
    /// It also requests user model, that can be use for computing of some values for aggregator. No specific functionality is forced on provided user model. More about usage of user models in <see cref="ISearchingStateGraphicsAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.  
    /// It runs through <c>SearchingStateGraphicsAggregators</c> and looks for appropriate graphics aggregator by pattern-matching their generic parameter <c>TSearchingState</c> with type of provided searching state.  
    /// When such aggregator is found, its aggregating method is called.  
    /// </summary>
    /// <param name="searchingState">Searching state which graphics is to be aggregated.</param>
    /// <param name="userModel">User model that is used for aggregation of searching state graphics.</param>
    /// <param name="collectorForAggregatedObjects">Collector for aggregated searching state graphic objects.</param>
    /// <param name="cancellationToken">Cancellation token for cancellation of aggregation.</param>
    /// <typeparam name="TSearchingState">Type of searching state which graphics is to be aggregated. It is used for appropriate aggregator searching.</typeparam>
    /// <returns>Result of aggregation.</returns>
    public AggregationResult AggregateSearchingStateGraphics<TSearchingState>(TSearchingState searchingState,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken = null)
        where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes>
    {
        foreach (var graphicsAggregator in SearchingStateGraphicsAggregators)
        {
            if (graphicsAggregator is
                ISearchingStateGraphicsAggregator<TSearchingState, TVertexAttributes, TEdgeAttributes>
                searchingStateGraphicsAggregator)
            {
                searchingStateGraphicsAggregator.AggregateGraphics(searchingState, userModel, collectorForAggregatedObjects, cancellationToken);
                return AggregationResult.Aggregated;
            }
        }
        return AggregationResult.NoUsableAggregatorFound;
    }
}
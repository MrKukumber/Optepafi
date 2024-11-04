using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Implementations;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;

/// <summary>
/// Represents one particular path searching algorithm.
///
/// It implements <c>ISearchingAlgorithm</c> interface and this class should be the only one implementing it.
/// All searching algorithm types should derive from this class.
/// For more information of searching algorithms see <see cref="ISearchingAlgorithm"/> iterface.
///
/// One of essential properties of searching algorithms is that only one execution can use graph instance at once.
/// That means it must be ensured that no one else has access to this graph while it is used by searching process.
/// This property of unique access to graph instance is ensured by:
/// 
/// - implementation of method for execution of searching algorithm by this class. It locks graph instance until the end of algorithms execution.  
/// - executors returned by this class which has its own mechanism for locking graph for itself. For more information no executors locking of graphs see <see cref="ISearchingExecutor"/>.  
/// </summary>
/// <typeparam name="TConfiguration">Type of configuration that is used in searching algorithm to adjust its execution.</typeparam>
public abstract class SearchingAlgorithm<TConfiguration> : ISearchingAlgorithm where TConfiguration : IConfiguration
{
    /// <inheritdoc cref="ISearchingAlgorithm.Name"/>
    public abstract string Name { get; }
    
    /// <inheritdoc cref="ISearchingAlgorithm.Implementations"/>
    public abstract ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; }

    /// <summary>
    /// Default configuration to be used in execution of searching algorithm.
    /// </summary>
    public abstract TConfiguration DefaultConfiguration { get; }

    /// <inheritdoc cref="ISearchingAlgorithm.DefaultConfigurationDeepCopy"/>
    IConfiguration ISearchingAlgorithm.DefaultConfigurationDeepCopy => DefaultConfiguration.DeepCopy();
    
    /// <inheritdoc cref="ISearchingAlgorithm.DoesRepresentUsableMapRepreUserModelCombination{TVertexAttributes,TEdgeAttributes}"/>
    public bool DoesRepresentUsableMapRepreUserModelCombination<TVertexAttributes, TEdgeAttributes>(IMapRepreRepresentative<IMapRepre> mapRepreRep, IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType) 
        where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        var graphRepresentative = mapRepreRep.GetCorrespondingGraphRepresentative<IVertexAttributes, IEdgeAttributes>();
        foreach (var implementation in Implementations)
        {
            if (implementation.DoesRepresentUsableGraph(graphRepresentative) && implementation.DoesRepresentUsableUserModel(userModelType))
                return true;
        }
        return false;
    }
    
    /// <inheritdoc cref="ISearchingAlgorithm.ExecuteSearch{TVertexAttributes,TEdgeAttributes}"/>
    public IPath<TVertexAttributes, TEdgeAttributes>[] ExecuteSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IList<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>,TVertexAttributes, TEdgeAttributes>> userModels,
        IConfiguration configuration,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation is ISearchingAlgorithmImplementation<TConfiguration> impl && implementation.IsUsableGraph(graph) && implementation.AreUsableUserModels(userModels))
            {
                lock (graph)
                {
                    if(configuration is TConfiguration config)
                        return impl.SearchForPaths(track, graph, userModels, config, progress, cancellationToken);
                    //TODO: log wrong configuration type
                    return impl.SearchForPaths(track, graph, userModels, DefaultConfiguration, progress, cancellationToken);
                }
            }
        }
        throw new ArgumentException("No implementation is able to use provided graph or any of provided user models. Did you forget to check their type usability before execution?");
    }

    /// <inheritdoc cref="ISearchingAlgorithm.GetExecutor{TVertexAttributes,TEdgeAttributes}"/>
    public ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IConfiguration configuration)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation is ISearchingAlgorithmImplementation<TConfiguration> impl && implementation.IsUsableGraph(graph) && implementation.IsUsableUserModel(userModel))
            {
                if(configuration is TConfiguration config)
                    return impl.GetExecutor(graph, userModel, config);
                //TODO: log wrong configuration type
                return impl.GetExecutor(graph, userModel, DefaultConfiguration);
            }
        }
        throw new ArgumentException("No implementation is able to use provided graph or provided user model. Did you forget to check their type usability before execution?");
    }
}




















    /// <summary>
    /// Method that checks whether there is some implementation of algorithm that can use map representation type represented by provided representative.
    /// It checks if it possesses the correct functionality.
    /// </summary>
    /// <param name="mapRepreRep">Representative of map representation type that is checked.</param>
    /// <returns>True if any of implementations can use represented map representation type. False otherwise.</returns>
    // sealed bool DoesRepresentUsableMapRepre(IMapRepreRepresentative<IMapRepre> mapRepreRep)
    // {
        // var graphRepresentative = mapRepreRep.GetCorrespondingGraphRepresentative<IVertexAttributes, IEdgeAttributes>();
        // foreach (var implementation in Implementations)
        // {
            // if (implementation.DoesRepresentUsableGraph(graphRepresentative))
                // return true;
        // }
        // return false;
    // }

    /// <summary>
    /// Method that checks whether there is some implementation of algorithm that can use user model type represented by provided <c>IUserModelTyp</c>.
    /// It checks if it possesses the correct functionality.
    /// </summary>
    /// <param name="computingUserModelType">Represents computing user model type that is checked.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes bounded to tested user model type.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes bounded to tested user model type.</typeparam>
    /// <returns>True, if any of implementations can use represented user model type.False otherwise.</returns>
    // sealed bool DoesRepresentUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> computingUserModelType)
        // where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    // {
        // foreach (var implementation in Implementations)
        // {
            // if (implementation.DoesRepresentUsableUserModel(computingUserModelType)) 
                // return true;
        // }
        // return false;
    // }

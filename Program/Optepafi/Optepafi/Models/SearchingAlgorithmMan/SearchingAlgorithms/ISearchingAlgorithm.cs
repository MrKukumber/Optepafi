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
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;

/// <summary>
/// Represents one particular path searching algorithm. 
/// It contains collection of its implementations. Each implementation can require other graphs functionalities.
/// Searching is done upon graph which satisfies functionality conditions.
/// Computing of weights for edges is done by provided user model to the algorithm. Weights of graphs edges are not computed before algorithm execution. Every algorithm should check at first that weight of specific edge is computed already. If it is not, it has to let user model to compute this weight and set it to the edge during run of its execution.
/// Before execution of algorithm should be run <see cref="DoesRepresentUsableMapRepreUserModelCombination{TVertexAttributes,TEdgeAttributes}"/> method to check if given graph - user model combination is usable for this algorithm.
/// Methods of searching algorithm should not be called directly from logic of application (ModelViews/ViewModels). <see cref="SearchingAlgorithmManager"/> should be used instead.
/// Each searching algorithm should be singleton and its instance presented in <see cref="SearchingAlgorithmManager"/> as viable option.
/// <para>
/// IMPORTANT!!! When algorithm is executing the graphs state will become inconsistent. That means that in time of execution it can not be used by other process than the current one. 
/// When algorithm is done with searching for path, it has to let graph to clean itself so it can be returned to its consistent state.
/// The unique look at graph instance is secured by:
/// - implementation of this interfaces sealed method for execution of searching algorithm. It locks graph instance until the end of algorithms execution.
/// - executors returned by this algorithm which has its own mechanism for locking graph for itself. For more information see <see cref="ISearchingExecutor"/>.
/// </para>
/// Each algorithm implementation should have good knowledge of graph functionalities it uses. Wrong usage can end up in bad behaviour of graph. 
/// </summary>
public interface ISearchingAlgorithm
{
    string Name { get; }
    /// <summary>
    /// Collection of usable implementations of the algorithm.
    /// </summary>
    protected ISearchingAlgoritmImplementation[] Implementations { get; }

    /// <summary>
    /// Method that checks whether there is some implementation of algorithm that can use both  map representation type and user model type represented by provided representatives.
    /// It checks if they possess the correct functionality.
    /// </summary>
    /// <param name="mapRepreRep">Representative of map representation type that is checked.</param>
    /// <param name="userModelType">Represents computing user model type that is checked.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes bounded to tested user model type.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes bounded to tested user model type.</typeparam>
    /// <returns>True if any of implementations can use both represented map representation type and user model type. False otherwise</returns>
    sealed bool DoesRepresentUsableMapRepreUserModelCombination<TVertexAttributes, TEdgeAttributes>(IMapRepreRepresentative<IMapRepre> mapRepreRep, IUserModelType<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType) 
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
    
    
    /// <summary>
    /// This method executes searching of algorithm on provided graph for each leg of given track each time with respect to one of provided user models.
    /// When usable implementation is found the lock on graph is requested and after its acquisition execution of implementation takes place. 
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="graph">Graph on which should be searching executed.</param>
    /// <param name="userModels">Collection of computing user models used in searching executions.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Collection of resulting found paths. Merged paths for legs of track are returned in order of corresponding user models.</returns>
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph or any of provided user models.</exception>
    sealed IPath<TVertexAttributes, TEdgeAttributes>[] ExecuteSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IList<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>,TVertexAttributes, TEdgeAttributes>> userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation.IsUsableGraph(graph) && implementation.AreUsableUserModels(userModels))
            {
                lock (graph)
                {
                    return implementation.SearchForPaths(track, graph, userModels, progress, cancellationToken);
                }
            }
        }
        throw new ArgumentException("No implementation is able to use provided graph or any of provided user models. Did you forget to check their type usability before execution?");
    }

    /// <summary>
    /// Method for retrieving searching algorithm executor.
    /// This executor will lock provided graph for itself so it should be disposed immediately after end of its usage.
    /// </summary>
    /// <param name="graph">Graph upon which will executor look for paths.</param>
    /// <param name="userModel">Computing user model which executor uses for path finding.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They  are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Executor of this searching algorithm.</returns>
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph or user model.</exception>
    sealed ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation.IsUsableGraph(graph) && implementation.IsUsableUserModel(userModel))
            {
                return implementation.GetExecutor(graph, userModel);
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

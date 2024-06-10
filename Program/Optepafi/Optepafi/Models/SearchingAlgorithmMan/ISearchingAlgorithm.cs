using System;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Implementations;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Represents one particular path searching algorithm. 
/// It contains collection of its implementations. Each implementation can require other graphs functionalities.
/// Searching is done upon graph which satisfies functionality conditions.
/// Computing of weights for edges is done by provided user model to the algorithm. Weights of graphs edges are not computed before algorithm execution. Every algorithm should check at first that weight of specific edge is computed already. If it is not, it has to let user model to compute this weight and set it to the edge during run of its execution.
/// Before execution of algorithm should be run <see cref="DoesRepresentUsableMapRepre"/> method to check if given graph is usable for this algorithm.
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
    /// Method that checks whether there is some implementation of algorithm that can use map representation type represented by provided representative.
    /// It checks if it possesses the correct functionality.
    /// </summary>
    /// <param name="mapRepreRep">Representative of map representation type that is checked.</param>
    /// <returns>True if any of implementation can use represented map representation type.</returns>
    sealed bool DoesRepresentUsableMapRepre(IMapRepreRepresentative<IMapRepre> mapRepreRep)
    {
        var definedFunctionalityMapRepreRep = mapRepreRep.GetCorrespondingGraphRepresentative<IVertexAttributes, IEdgeAttributes>();
        foreach (var implementation in Implementations)
        {
            if (implementation.DoesRepresentUsableMapRepre(definedFunctionalityMapRepreRep))
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
    /// <returns>Collection of resulting found paths. Path collections (for legs of track) are returned in order of corresponding user models.</returns>
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph.</exception>
    sealed ClassicColoredPath[][] ExecuteSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes>[] userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
            foreach (var implementation in Implementations)
            {
                if (implementation.IsUsableGraph(graph))
                {
                    lock (graph)
                    {
                        return implementation.SearchForPaths(track, graph,  userModels,progress, cancellationToken);
                    }
                }
            }
            throw new ArgumentException("No implementation is able to use provided graph. Did you forget to check its type usability before execution?");
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
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph.</exception>
    sealed ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation.IsUsableGraph(graph))
            {
                return implementation.GetExecutor(graph,userModel);
            }
        }
        throw new ArgumentException("No implementation is able to use provided graph. Did you forget to check its type usability before execution?");
    }
    
}
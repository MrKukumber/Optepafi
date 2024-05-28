using System;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Implementation of an algorithm. Each implementation is tied to some particular searching algorithm and it should obey its principles. All algorithm implementations are collected and presented in searching algorithm singleton.
/// Every implementation defines its requirements for used map representations functionality in process of searching. This requirements can be tested by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> and <see cref="DoesRepresentUsableMapRepre"/> methods.
/// For more information about searching algorithms see <see cref="ISearchingAlgorithm"/>.
/// </summary>
public interface ISearchingAlgoritmImplementation
{
    /// <summary>
    /// For provided graph representative resolves whether represented graph type satisfies implementations functionality requirements. 
    /// This test has to correspond to test provided by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> method.
    /// Test is done on no particular vertex-edge attribute types.
    /// </summary>
    /// <param name="graphRepresentative">Representative of graph whose functionalities are tested.</param>
    /// <returns>True if all requirements are satisfied. Otherwise false.</returns>
    bool DoesRepresentUsableMapRepre(IGraphRepresentative<IGraph<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes>
            graphRepresentative);

    /// <summary>
    /// Checks if provided graphs functionality satisfies implementations requirements.
    /// This test has to correspond to test provided by <see cref="DoesRepresentUsableMapRepre"/> method.
    /// </summary>
    /// <param name="graph">Graph to be tested for its functionalities.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in vertices of graph.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in edges of graph.</typeparam>
    /// <returns>True if all requirements are satisfied. Otherwise false.</returns>
    bool IsUsableGraph<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Searches for paths on provided map representation for each leg of given track each time with respect to one of provided user models.
    /// It should be secured by caller that this will be the only progress which uses provided graph instance.
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="graph">Graph on which should be searching executed.</param>
    /// <param name="userModels">Collection of computing user models used in searching executions.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Collection of resulting found paths. Path collections (for legs of track) are returned in order of corresponding user models.</returns>
    Path[][] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes>[] userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Method for retrieving searching algorithm executor of this implementation.
    /// This executor will lock provided graph for itself so it should be disposed immediately after end of its usage.
    /// </summary>
    /// <param name="graph">Graph upon which will executor look for paths</param>
    /// <param name="userModel">Computing user model which executor uses for path finding.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They  are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Executor of this implementation.</returns>
    sealed ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return new SearchingExecutor<TVertexAttributes, TEdgeAttributes>(graph, userModel, ExecutorSearch);
    }

    /// <summary>
    /// Method used as delegate for searching executor. It provides implementations path finding algorithm execution logic for executor.
    /// It should be more or less identical to <see cref="SearchForPaths{TVertexAttributes,TEdgeAttributes}"/> methods execution with only one user model provided. 
    /// </summary>
    /// <returns></returns>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="graph">Graph on which should be searching executed.</param>
    /// <param name="userModel">Computing user model used in searching executions.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user model.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user model.</typeparam>
    /// <returns>Collection of resulting found paths for legs of track.</returns>
    protected Path[] ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;


}
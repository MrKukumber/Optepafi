using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations;

/// <summary>
/// Implementation of an algorithm.
///
/// Each implementation is tied to some particular searching algorithm and it should obey its principles. All algorithm implementations are collected and presented in searching algorithm singleton.  
/// Every implementation defines its requirements for used map representations functionality in process of searching. This requirements can be tested by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> and <see cref="DoesRepresentUsableGraph"/> methods.  
/// For more information about searching algorithms see <see cref="ISearchingAlgorithm"/>.  
/// </summary>
public interface ISearchingAlgorithmImplementation
{
    /// <summary>
    /// For provided graph representative resolves whether represented graph type satisfies implementations functionality requirements.
    /// 
    /// This test has to correspond to test provided by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> method.  
    /// Test is done on no particular vertex-edge attribute types.  
    /// </summary>
    /// <param name="graphRepresentative">Representative of graph type whose functionalities are tested.</param>
    /// <returns>True if all requirements are satisfied. Otherwise false.</returns>
    bool DoesRepresentUsableGraph(IGraphRepresentative<IGraph<IVertexAttributes, IEdgeAttributes>, IConfiguration, IVertexAttributes, IEdgeAttributes>
            graphRepresentative);

    /// <summary>
    /// For provided user model type resolves whether represented user model type satisfies implementations functionality requirements.
    /// 
    /// This test has to correspond to test provided by <see cref="IsUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method.  
    /// Test is done on particular vertex-edge attribute types.  
    /// </summary>
    /// <param name="userModelType">Representative of user model type whose functionalities are tested.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in represented user model type.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in represented user model type.</typeparam>
    /// <returns>True, if all requirements are satisfied. False otherwise.</returns>
    bool DoesRepresentUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Checks whether provided graphs functionality satisfies implementations requirements.
    /// 
    /// This test has to correspond to test provided by <see cref="DoesRepresentUsableGraph"/> method.
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
    /// Checks whether provided user models functionality satisfies implementations requirements.
    /// 
    /// This test has to correspond to test provided by <see cref="DoesRepresentUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method.  
    /// </summary>
    /// <param name="userModel">User model to be tested for its functionalities.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used by user model.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used by user model.</typeparam>
    /// <returns>True, if all requirements are satisfied. False otherwise.</returns>
    bool IsUsableUserModel<TVertexAttributes, TEdgeAttributes>(
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Checks whether functionalities of all provided user models satisfy implementations requirements.
    /// 
    /// Default implementation iterates through all user models and calls <see cref="IsUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method on them.  
    /// </summary>
    /// <param name="userModels"></param>
    /// <typeparam name="TVertexAttributes"></typeparam>
    /// <typeparam name="TEdgeAttributes"></typeparam>
    /// <returns></returns>
    bool AreUsableUserModels<TVertexAttributes, TEdgeAttributes>(
        IEnumerable<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var userModel in userModels)
        {
            if (!IsUsableUserModel(userModel)) return false;
        }
        return true;
    }

    /// <summary>
    /// Searches for paths on provided map representation for each leg of given track each time with respect to one of provided user models.
    ///
    /// Functionalities of user models and graph should be tested before calling of this method. 
    /// It should be secured by caller that this will be the only progress which uses provided graph instance.  
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="graph">Graph on which should be searching executed.</param>
    /// <param name="userModels">Collection of computing user models used in searching executions.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Collection of resulting found paths. Merged paths for legs of track are returned in order of corresponding user models.</returns>
    IPath<TVertexAttributes, TEdgeAttributes>[] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IList<IComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Method for retrieving searching algorithm executor of this implementation.
    ///
    /// This executor will lock provided graph for itself so it should be disposed immediately after end of its usage.  
    /// </summary>
    /// <param name="graph">Graph upon which will executor look for paths</param>
    /// <param name="userModel">Computing user model which executor uses for path finding.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They  are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Executor of this implementation.</returns>
    ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return new SearchingExecutor<TVertexAttributes, TEdgeAttributes>(graph, userModel, ExecutorSearch);
    }

    /// <summary>
    /// Method used as delegate for searching executor.
    /// 
    /// It provides implementations path finding algorithm execution logic for executor.  
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
    /// <returns>Merged paths for legs of track.</returns>
    protected IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;


}
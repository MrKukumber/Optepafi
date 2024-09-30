using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations;

/// <summary>
/// Implementation of an algorithm.
///
/// Each implementation is tied to some particular searching algorithm and it should obey its principles. All algorithm implementations are collected and presented in searching algorithm singleton.  
/// For more information about searching algorithms see <see cref="ISearchingAlgorithm{TConfiguration}"/>.  
/// </summary>
public interface ISearchingAlgorithmImplementation<in TConfiguration> : ISearchingAlgorithmImplementationRequirementsIndicator where TConfiguration : IConfiguration
{

    /// <summary>
    /// Searches for paths on provided map representation for each leg of given track each time with respect to one of provided user models.
    ///
    /// Functionalities of user models and graph should be tested before calling of this method. 
    /// It should be secured by caller that this will be the only progress which uses provided graph instance.  
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="graph">Graph on which should be searching executed.</param>
    /// <param name="userModels">Collection of computing user models used in searching executions.</param>
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Collection of resulting found paths. Merged paths for legs of track are returned in order of corresponding user models.</returns>
    IPath<TVertexAttributes, TEdgeAttributes>[] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IList<IComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels,
        TConfiguration configuration,
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
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They  are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Executor of this implementation.</returns>
    ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        TConfiguration configuration)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return new SearchingExecutor<TConfiguration, TVertexAttributes, TEdgeAttributes>(graph, userModel, ExecutorSearch, configuration);
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
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user model.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They are used for retrieving weights of edges from user model.</typeparam>
    /// <returns>Merged paths for legs of track.</returns>
    protected IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        TConfiguration configuration,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;


}
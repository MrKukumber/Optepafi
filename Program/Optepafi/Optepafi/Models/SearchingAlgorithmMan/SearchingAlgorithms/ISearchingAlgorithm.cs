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

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;

/// <summary>
/// Represents one particular path searching algorithm.
/// 
/// It contains collection of its implementations. Each implementation can require other graphs functionalities.  
/// Searching is done upon graph which satisfies functionality conditions.  
/// Computing of weights for edges is done by provided user model to the algorithm. Weights of graphs edges are not computed before algorithm execution. Every algorithm should check at first that weight of specific edge is computed already. If it is not, it has to let user model to compute this weight and set it to the edge during run of its execution.
/// Execution of the algorithm can be adjusted by providing configuration object of specific type.
/// Before execution of algorithm should be run <see cref="DoesRepresentUsableMapRepreUserModelCombination{TVertexAttributes,TEdgeAttributes}"/> method to check if given graph - user model combination is usable for this algorithm.  
/// Methods of searching algorithm should not be called directly from logic of application (ModelViews/ViewModels). <see cref="SearchingAlgorithmManager"/> should be used instead.  
/// Each searching algorithm should be singleton and its instance presented in <see cref="SearchingAlgorithmManager"/> as viable option.
/// 
/// IMPORTANT!!! When algorithm is executing the graphs state will become inconsistent. That means that in time of execution it can not be used by other process than the current one.  
/// When algorithm is done with searching for path, it has to let graph to clean itself so it can be returned to its consistent state.
/// 
/// Also this interface should not be implemented right away. <see cref="SearchingAlgorithm{TConfiguration}"/> class should be derived instead.
/// Each algorithm implementation should have good knowledge of graph functionalities it uses. Wrong usage can end up in bad behaviour of graph.   
/// </summary>
public interface ISearchingAlgorithm
{
    
    string Name { get; }
    /// <summary>
    /// Collection of usable implementations of the algorithm.
    /// </summary>
    protected ISearchingAlgorithmImplementationRequirementsIndicator[] Implementations { get; }
    
    /// <summary>
    /// Default configuration to be used in execution of searching algorithm.
    /// </summary>
    public IConfiguration DefaultConfigurationDeepCopy { get; }

    /// <summary>
    /// Method that checks whether there is some implementation of algorithm that can use both map representation type and user model type represented by provided representatives. /// 
    /// It checks if they possess the correct functionality.
    /// </summary>
    /// <param name="mapRepreRep">Representative of map representation type that is checked.</param>
    /// <param name="userModelType">Represents computing user model type that is checked.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes bounded to tested user model type.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes bounded to tested user model type.</typeparam>
    /// <returns>True if any of implementations can use both represented map representation type and user model type. False otherwise</returns>
    bool DoesRepresentUsableMapRepreUserModelCombination<TVertexAttributes, TEdgeAttributes>(
        IMapRepreRepresentative<IMapRepre> mapRepreRep,
        IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// This method executes searching of algorithm on provided graph for each leg of given track each time with respect to one of provided user models.
    /// 
    /// When usable implementation is found the lock on graph is requested and after its acquisition execution of implementation takes place. 
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
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph or any of provided user models.</exception>
    IPath<TVertexAttributes, TEdgeAttributes>[] ExecuteSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IList<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels,
        IConfiguration configuration,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
         

    /// <summary>
    /// Method for retrieving searching algorithm executor.
    /// 
    /// This executor will lock provided graph for itself so it should be disposed immediately after end of its usage.
    /// </summary>
    /// <param name="graph">Graph upon which will executor look for paths.</param>
    /// <param name="userModel">Computing user model which executor uses for path finding.</param>
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes used in algorithms execution. They are used for retrieving weights of edges from user models.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes used in algorithms execution. They  are used for retrieving weights of edges from user models.</typeparam>
    /// <returns>Executor of this searching algorithm.</returns>
    /// <exception cref="ArgumentException">When no implementation is able to use provided graph or user model.</exception>
    ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IConfiguration configuration)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

}
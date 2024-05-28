using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Singleton class use for managing execution of path finding algorithms. It contains set of all supported searching algorithms. It is main channel between operations with searching algorithms and applications logic (ViewModels/ModelViews).
/// It implements supporting methods for work with path finding algorithms. All algorithms should be preferably used and managed through this singleton.
/// All operations provided by this class are thread safe as long as same method arguments are not use concurrently multiple times.
/// </summary>
public class SearchingAlgorithmManager : 
    ITemplateGenericVisitor<(SearchingAlgorithmManager.Result[], Path[]?[]),(Leg[], ISearchingAlgorithm, IMapRepre, IUserModel[], IProgress<ISearchingReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<Path[], (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel, IProgress<ISearchingReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre, IUserModel)>
{
    public static SearchingAlgorithmManager Instance { get; } = new();
    private SearchingAlgorithmManager(){}

    /// <summary>
    /// Set of all usable searching algorithms.
    /// </summary>
    public ISet<ISearchingAlgorithm> SearchingAlgorithms { get; } =
        ImmutableHashSet.Create<ISearchingAlgorithm>(SmileyFaceDrawing.Instance);

    /// <summary>
    /// Returns all algorithms that are able to use map representation type represented by provided representative.
    /// Map representation type must satisfy set of contracts that algorithm requires so it might work correctly.
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation.</param>
    /// <returns>Set of usable algorithms for tested map representation type.</returns>
    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        IMapRepreRepresentative<IMapRepre> mapRepreRep)
    {
        HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        foreach (var searchingAlgorithm in SearchingAlgorithms)
        {
            if (searchingAlgorithm.DoesRepresentUsableMapRepre(mapRepreRep)) usableAlgorithms.Add(searchingAlgorithm);
        }
        return usableAlgorithms;
    }

    /// <summary>
    /// Returns all algorithms that are able to use at least one map representation type represented by provided collection of representatives.
    /// It do it so by calling its overload <see cref="GetUsableAlgorithmsFor(Optepafi.Models.MapRepreMan.MapRepreReps.IMapRepreRepresentative{Optepafi.Models.MapRepreMan.MapRepres.IMapRepre})"/> for each of provided representative and then joins results. 
    /// </summary>
    /// <param name="mapRepreReps">Representatives of tested map representations.</param>
    /// <returns>Set of usable algorithms for set of tested map representation types.</returns>
    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        IMapRepreRepresentative<IMapRepre>[] mapRepreReps)
    {
        HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        foreach (var mapRepreRep in mapRepreReps)
        {
            usableAlgorithms.UnionWith(GetUsableAlgorithmsFor(mapRepreRep));
        }
        return usableAlgorithms;
    }

    /// <summary>
    /// Checks if map representation type represented by provided representative is usable for provided searching algorithm.
    /// Map representation type must satisfy set of contracts that algorithm requires so it might work correctly.
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation type.</param>
    /// <param name="algorithm">Algorithm which tests map representation types usability.</param>
    /// <returns>True if map representation type is usable in provided algorithm. Otherwise false.</returns>
    public bool DoesRepresentUsableMapRepreFor(IMapRepreRepresentative<IMapRepre> mapRepreRep,
        ISearchingAlgorithm algorithm)
    {
        return algorithm.DoesRepresentUsableMapRepre(mapRepreRep);
    }
    
    
    public enum Result{Ok = 1, NotComputingUserModelOrNotTiedToTemplate }


    /// <summary>
    /// This method executes searching of provided algorithm on provided map representation for each leg of given track with respect to provided user model.
    /// This method uses generic visitor pattern on template in order to gain vertex and edge attribute types which are then used in algorithms execution.
    /// Provided user models and map representation must be tied to this template and its vertex/edge attribute types. If they are not, the exception is thrown.
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="algorithm">Algorithm to be used for path finding.</param>
    /// <param name="mapRepre">Map representation on which should be searching executed.</param>
    /// <param name="userModel">User model used in searching execution.</param>
    /// <param name="template">Template which define vertex and edge attribute types that should be used in algorithm execution.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <returns>Collection of resulting found paths for legs of track.</returns>
    /// <exception cref="ArgumentException">Thrown if eiter map representation is not graph or user model is not computing one or if either of them is not tied to provided template.</exception>
    public Path[] ExecuteSearch(Leg[] track, ISearchingAlgorithm algorithm, IMapRepre mapRepre, IUserModel userModel,
        ITemplate template, IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        return template
            .AcceptGeneric<Path[], (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel, IProgress<ISearchingReport>?, CancellationToken?)>
                (this, (track, algorithm, mapRepre, userModel, progress, cancellationToken));
    }

    Path[]  ITemplateGenericVisitor<Path[], (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel, IProgress<ISearchingReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>
        (TTemplate template, (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel, IProgress<ISearchingReport>?, CancellationToken?) otherParams) 
    {
        
        var (track, algorithm, mapRepre, userModel, progress, cancellationToken) = otherParams;
        if (userModel is IComputingUserModel<TVertexAttributes, TEdgeAttributes> computingUserModel)
        {
            if (mapRepre is IGraph<TVertexAttributes, TEdgeAttributes> graph)
            {
                return algorithm.ExecuteSearch(track, graph, [computingUserModel], progress, cancellationToken)[0];
            }
            throw new ArgumentException("Provided map representation is not a graph ot it is not tide to given template.");
        }
        throw new ArgumentException("Provided user model is not computing one or it is not tied to given template.");
    }

    /// <summary>
    /// This method tries to execute provided searching algorithm on provided map representation for each leg of given track, each time with respect to other provided user model.
    /// In the first step it tests all user models if they are able to compute weights for provided vertex-edge attributes combinations.
    /// Then for those which are able to do so it sequentially executes search of paths on given map representation respectively to each of them.
    /// If provided map representation turns out to be not usable for given searching algorithm, the exception proper exception is thrown. The usability of maps representation should be tested before calling this method.
    /// In the end are results and found paths returned in order of their corresponding user models.
    ///
    /// This method uses generic visitor pattern on template in order to gain vertex and edge attribute types which are then used in algorithms execution.
    /// Provided user models and map representation must be tied to this template and its vertex/edge attribute types.
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="algorithm">Algorithm to be used for path finding.</param>
    /// <param name="mapRepre">Map representation on which should be searching executed.</param>
    /// <param name="userModels">Collection of user models used in searching executions.</param>
    /// <param name="template">Template which define vertex and edge attribute types that should be used in algorithm execution.</param>
    /// <param name="resultingPaths">Out parameter that returns resulting found paths. Path collections (for legs of track) are returned in order of corresponding user models.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <returns>Collection of algorithm execution results.</returns>
    /// <exception cref="ArgumentException">Thrown if map representation is not graph or it is not tied to provided template.</exception>
    public Result[] TryExecuteSearch(Leg[] track, ISearchingAlgorithm algorithm, IMapRepre mapRepre, 
        IUserModel[] userModels, ITemplate template, out Path[]?[] resultingPaths,
        IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        var (searchingExecutionResults, paths) = template.AcceptGeneric<(Result[], Path[]?[]), (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel[], IProgress<ISearchingReport>?, CancellationToken?)>(this, (track, algorithm, mapRepre, userModels, progress, cancellationToken));
        resultingPaths = paths;
        return searchingExecutionResults;
    }
    
    (Result[], Path[]?[]) ITemplateGenericVisitor<(Result[], Path[]?[]), (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel[], IProgress<ISearchingReport>?, CancellationToken?)>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel[], IProgress<ISearchingReport>?, CancellationToken?) otherParams)
    {
        var (track, algorithm, mapRepre, userModels, progress, cancellationToken) = otherParams;
        
        Result[] results = new Result[userModels.Length];
        List<IComputingUserModel<TVertexAttributes, TEdgeAttributes>> usableUserModels = new();
        for(int i = 0; i < userModels.Length; ++i)
        {
            if (userModels[i] is IComputingUserModel<TVertexAttributes, TEdgeAttributes> computingUserModel)
            {
                usableUserModels.Add(computingUserModel);
            }
            else results[i] = Result.NotComputingUserModelOrNotTiedToTemplate;
        }
        
        Path[][] foundPaths;

        if (mapRepre is IGraph<TVertexAttributes, TEdgeAttributes> definedFunctionalityMapRepre)
            foundPaths = algorithm.ExecuteSearch(track, definedFunctionalityMapRepre, usableUserModels.ToArray(),
                progress, cancellationToken);
        else
            throw new ArgumentException("Provided map representation is not a graph or it is not tied to given template.");
        
        Path[]?[] resultingPaths = new Path[]?[userModels.Length];
        
        int j = 0;
        for (int i = 0; i < userModels.Length; ++i)
        {
            if (results[i] == 0)
            {
                results[i] = Result.Ok;
                resultingPaths[i] = foundPaths[j++];
            }
            else
            {
                resultingPaths[i] = null;
            }
        }
        return (results, resultingPaths);
    }

    
    /// <summary>
    /// Returns path finding algorithms executor instantiated with provided map representation and user model, both tied to given templaate.
    /// This executor will lock provided map representation for itself so it should be disposed immediately after end of its usage.
    /// This method uses generic visitor pattern on template in order to gain vertex and edge attribute types which are then used in algorithms execution.
    /// Provided user models and map representation must be tied to this template and its vertex/edge attribute types. If they are not, the exception is thrown.
    /// </summary>
    /// <param name="algorithm">Algorithm whose executor is provided.</param>
    /// <param name="mapRepre">Map representation upon which will executor look for paths.</param>
    /// <param name="userModel">User model which executor uses for path finding.</param>
    /// <param name="template">Template which represents vertex and edge attribute types used in path finding algorithm which is used by executor.</param>
    /// <returns>Executor of provided searching algorithm.</returns>
    /// <exception cref="ArgumentException">Thrown if eiter map representation is not graph or user model is not computing one or if either of them is not tied to provided template.</exception>
    public  ISearchingExecutor GetExecutorOf(ISearchingAlgorithm algorithm, IMapRepre mapRepre, IUserModel userModel, ITemplate template)
    {
        return  template.AcceptGeneric<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre, IUserModel)>(this, (algorithm, mapRepre,userModel));
    }
    
    ISearchingExecutor ITemplateGenericVisitor<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre,IUserModel)>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, (ISearchingAlgorithm, IMapRepre, IUserModel) otherParams)
    {
        var (algorithm, mapRepre, userModel) = otherParams;
        if (userModel is IComputingUserModel<TVertexAttributes, TEdgeAttributes> computingUserModel)
        {
            if (mapRepre is IGraph<TVertexAttributes, TEdgeAttributes> definedFunctionalityMapRepre)
            {
                var executor = algorithm.GetExecutor(definedFunctionalityMapRepre, computingUserModel);
                return executor;
            }
            throw new ArgumentException("Provided map representation is not a graph ot it is not tide to given template.");
        }
        throw new ArgumentException("Provided user model is not computing one or it is not tied to given template.");
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Singleton class use for managing execution of path finding algorithms.
///
/// It contains set of all supported searching algorithms. It is main channel between operations with searching algorithms and applications logic (ViewModels/ModelViews).  
/// It implements supporting methods for work with path finding algorithms. All algorithms should be preferably used and managed through this singleton.  
/// All operations provided by this class are thread safe as long as same method arguments are not use concurrently multiple times.  
/// </summary>
public class SearchingAlgorithmManager : 
    ITemplateGenericVisitor<(SearchingAlgorithmManager.SearchResult[], IPath?[]),(Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<IPath, (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration)>,
    ITemplateGenericVisitor<HashSet<ISearchingAlgorithm>,(IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>,ITemplate>)>,
    ITemplateGenericVisitor<int, (IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>,ITemplate>, ISearchingAlgorithm)>,
    IGraphGenericVisitor<(SearchingAlgorithmManager.SearchResult[], IPath?[]), IVertexAttributes, IEdgeAttributes, (Leg[], ISearchingAlgorithm, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>,
    IGraphGenericVisitor<ISearchingExecutor, IVertexAttributes, IEdgeAttributes, (ISearchingAlgorithm, IUserModel<ITemplate>, IConfiguration)>
{
    public static SearchingAlgorithmManager Instance { get; } = new();
    private SearchingAlgorithmManager(){}

    /// <summary>
    /// Set of all usable searching algorithms.
    /// </summary>
    public ISet<ISearchingAlgorithm> SearchingAlgorithms { get; } =
        ImmutableHashSet.Create<ISearchingAlgorithm>(AStar.Instance, SmileyFacesDrawer.Instance);


    
    /// <summary>
    /// Returns all algorithms that are able to use both map representation type and user model type represented by provided representatives.
    /// 
    /// Map representation type and user model type must both satisfy set of contracts required by some of algorithms implementations so it could work correctly.  
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation type.</param>
    /// <param name="userModelType">Representative of tested user model type.</param>
    /// <returns>Set of usable algorithms for tested combination of map representation and user model types.</returns>
    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(IMapRepreRepresentative<IMapRepre> mapRepreRep, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    {
        return userModelType.AssociatedTemplate.AcceptGeneric<HashSet<ISearchingAlgorithm>,(IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>,ITemplate>)>(this, (mapRepreRep,userModelType));
    }
    HashSet<ISearchingAlgorithm> ITemplateGenericVisitor<HashSet<ISearchingAlgorithm>,(IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>,ITemplate>)>.GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        (IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>, ITemplate>) otherParams)
    {
        var (mapRepreRep, userModelType) = otherParams;
        HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        if (userModelType is IUserModelType< IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> computingUserModelType)
        {
            foreach(var graphCreator in mapRepreRep.GetGraphCreators<TVertexAttributes, TEdgeAttributes>())
                foreach (var searchingAlgorithm in SearchingAlgorithms)
                {
                    if (graphCreator.RevelationForSearchingAlgorithmMan(this, computingUserModelType, searchingAlgorithm))
                        usableAlgorithms.Add(searchingAlgorithm);
                }
        }
        return usableAlgorithms;
    }

    
    /// <summary>
    /// Returns all algorithms that are able to use at least one combination of map representation type and user model type represented by provided collections of representatives.  
    /// 
    /// It do it so by calling its overload <see cref="GetUsableAlgorithmsFor(Optepafi.Models.MapRepreMan.MapRepres.Representatives.IMapRepreRepresentative{Optepafi.Models.MapRepreMan.MapRepres.IMapRepre},Optepafi.Models.UserModelMan.UserModelReps.IUserModelType{Optepafi.Models.UserModelMan.UserModels.IUserModel{Optepafi.Models.TemplateMan.ITemplate},Optepafi.Models.TemplateMan.ITemplate})"/> for each of provided representative combination and then joins results.  
    /// </summary>
    /// <param name="mapRepreReps">Representatives of tested map representations types.</param>
    /// <param name="userModelTypes">Representatives of tested user model types.</param>
    /// <returns>Set of usable algorithms for set of tested map representation-user model types combinations.</returns>
    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(IEnumerable<IMapRepreRepresentative<IMapRepre>> mapRepreReps, IEnumerable<IUserModelType<IUserModel<ITemplate>, ITemplate>> userModelTypes)
        {
            HashSet<ISearchingAlgorithm> usableAlgorithms = new();
            foreach (var mapRepreRep in mapRepreReps)
            {
                foreach (var userModelType in userModelTypes)
                {
                    usableAlgorithms.UnionWith(GetUsableAlgorithmsFor(mapRepreRep, userModelType));
                }
            }
            return usableAlgorithms;
        }

    /// <summary>
    /// Checks whether map representation - user model types combination represented by provided representatives is usable for provided searching algorithm.
    /// 
    /// Represented types must satisfy set of contracts that algorithm requires so it might work correctly.  
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation type.</param>
    /// <param name="userModelType">Representative of tested user model type.</param>
    /// <param name="algorithm">Algorithm which tests map representation - user model types combination usability.</param>
    /// <returns>Index of relevant graph constructor of map representation, if provided combination of types is usable in provided algorithm. -1 otherwise.</returns>
    public int GetRelevantGraphCreatorIndexIfRepresentUsableMapRepreUserModelCombFor(IMapRepreRepresentative<IMapRepre> mapRepreRep, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType, ISearchingAlgorithm algorithm)
    {
        return userModelType.AssociatedTemplate.AcceptGeneric<int, (IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>, ITemplate>, ISearchingAlgorithm)>(this, (mapRepreRep, userModelType, algorithm));
    }
    int ITemplateGenericVisitor<int, (IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>, ITemplate>, ISearchingAlgorithm)>.GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, (IMapRepreRepresentative<IMapRepre>, IUserModelType<IUserModel<ITemplate>, ITemplate>, ISearchingAlgorithm) otherParams)
    {
        var (mapRepreRep, userModelType, algorithm) = otherParams;
        if (userModelType is IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes> , TTemplate> computingUserModelType)
        {
            var graphCreators = mapRepreRep.GetGraphCreators<TVertexAttributes, TEdgeAttributes>();
            for (int i = 0; i < mapRepreRep.GetGraphCreators<TVertexAttributes, TEdgeAttributes>().Length; ++i)
                if (graphCreators[i].RevelationForSearchingAlgorithmMan(this, computingUserModelType, algorithm)) return i;
        }
        return -1;
    }

    public bool AcceptGraphCreatorsRevelation<TVertex, TEdge, TTemplate, TVertexAttributes, TEdgeAttributes>(
        IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge> revealedGraphRep,
        IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> computingUserModelType,
        ISearchingAlgorithm algorithm)
        where TVertex : IVertex
        where TEdge : IEdge
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return algorithm.DoesRepresentUsableGraphUserModelCombination(revealedGraphRep, computingUserModelType);
    }
    
    public enum SearchResult{Ok = 1, NotComputingUserModelOrNotTiedToTemplate}


    /// <summary>
    /// This method executes searching of provided algorithm on provided map representation for each leg of given track with respect to provided user model.
    ///
    /// Execution of the algorithm can be adjusted by provided configuration.
    /// This method uses generic visitor pattern on user models associated template in order to gain vertex and edge attribute types which are then used in algorithms execution.  
    /// Provided user models and map representation must be tied to this template and its vertex/edge attribute types. If they are not, the exception is thrown.  
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="algorithm">Algorithm to be used for path finding.</param>
    /// <param name="mapRepre">Map representation on which should be searching executed.</param>
    /// <param name="userModel">User model used in searching execution. Its associated template defines vertex and edge attributes types used in algorithm execution.</param>
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <returns>Found merged paths for legs of track.</returns>
    /// <exception cref="ArgumentException">Thrown if eiter map representation is not graph or user model is not computing one or if graph is not tied to user models associated template.</exception>
    public IPath ExecuteSearch(Leg[] track, ISearchingAlgorithm algorithm, IMapRepre mapRepre, IUserModel<ITemplate> userModel, IConfiguration configuration, IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        return userModel.AssociatedTemplate.AcceptGeneric<IPath, (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>
                (this, (track, algorithm, mapRepre, userModel, configuration, progress, cancellationToken));
    }

    IPath  ITemplateGenericVisitor<IPath, (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>
        .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>
        (TTemplate template, (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration, IProgress<ISearchingReport>?, CancellationToken?) otherParams) 
    {
        
        var (track, algorithm, mapRepre, userModel, configuration,progress, cancellationToken) = otherParams;
        if (userModel is IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> computingUserModel)
        {
            if (mapRepre is IGraph<IAttributesBearingVertex<TVertexAttributes>, IAttributesBearingEdge<TEdgeAttributes>> graph)
            {
                return algorithm.ExecuteSearch(track, graph, [computingUserModel], configuration, progress, cancellationToken)[0];
            }
            throw new ArgumentException("Provided map representation is not a graph ot its vertices or edges bear attributes of given template.");
        }
        throw new ArgumentException("Provided user model is not computing one.");
    }

    /// <summary>
    /// This method tries to execute provided searching algorithm on provided map representation for each leg of given track, each time with respect to other provided user model.
    /// 
    /// In the first step it tests all user models if they are computing user models.  
    /// Then for those which are computational it sequentially executes search of paths on given map representation respectively to each of them.  
    /// If provided map representation turns out not to be a graph, the proper exception is thrown.  
    /// If provided map representation or any user model turn out to be not usable by searching algorithm, it throws proper exception. The usability of maps representation and user model types should be tested before calling this method.  
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
    /// <param name="resultingMergedPaths">Out parameter that returns resulting found paths. Path report collections (for legs of track) are returned in order of corresponding user models.</param>
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <returns>Collection of algorithm execution results.</returns>
    /// <exception cref="ArgumentException">Thrown if map representation is not graph or it is not tied to provided template.</exception>
    public SearchResult[] TryExecuteSearch(Leg[] track, ISearchingAlgorithm algorithm, IMapRepre mapRepre, 
        IUserModel<ITemplate>[] userModels, ITemplate template, out IPath?[] resultingMergedPaths, IConfiguration configuration, 
        IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        var (searchingExecutionResults, mergedPaths) = template.AcceptGeneric<(SearchResult[], IPath?[]), (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>(this, (track, algorithm, mapRepre, userModels, configuration, progress, cancellationToken));
        resultingMergedPaths = mergedPaths;
        return searchingExecutionResults;
    }
    
    (SearchResult[], IPath?[]) ITemplateGenericVisitor<(SearchResult[], IPath?[]), (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        (Leg[], ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?) otherParams)
    {
        var (track, algorithm, mapRepre, userModels, configuration, progress, cancellationToken) = otherParams;
        if (mapRepre is IGraph<IVertex, IEdge> graph)
            return graph.AcceptGeneric<(SearchResult[], IPath?[]), TVertexAttributes, TEdgeAttributes, IVertexAttributes, IEdgeAttributes, (Leg[], ISearchingAlgorithm, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>(this, (track, algorithm, userModels, configuration, progress, cancellationToken)); 
        throw new ArgumentException("Provided map representation is not a graph.");
            
    }

    (SearchResult[], IPath?[]) IGraphGenericVisitor<(SearchResult[], IPath?[]), IVertexAttributes, IEdgeAttributes, (Leg[], ISearchingAlgorithm, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?)>.
        GenericVisit<TGraph, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(TGraph graph, (Leg[], ISearchingAlgorithm, IUserModel<ITemplate>[], IConfiguration, IProgress<ISearchingReport>?, CancellationToken?) otherParams)
    {
        var (track, algorithm, userModels, configuration, progress, cancellationToken) = otherParams;
        
        SearchResult[] results = new SearchResult[userModels.Length];
        List<IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>> usableUserModels = new();
        for(int i = 0; i < userModels.Length; ++i)
        {
            if (userModels[i] is IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> usableUserModel)
            {
                usableUserModels.Add(usableUserModel);
            }
            else results[i] = SearchResult.NotComputingUserModelOrNotTiedToTemplate;
        }
        
        IPath[] foundPaths;

        foundPaths = usableUserModels.Count == 0 ? [] : algorithm.ExecuteSearch(track, graph, usableUserModels, configuration, progress, cancellationToken);

        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return ([], []);
        
        IPath?[] resultingPaths = new IPath?[userModels.Length];
        
        int j = 0;
        for (int i = 0; i < userModels.Length; ++i)
        {
            if (results[i] == 0)
            {
                results[i] = SearchResult.Ok;
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
    /// Returns path finding algorithms executor instantiated with provided map representation and user model, both tied to user models associated template.
    /// 
    /// This executor will lock provided map representation for itself so it should be disposed immediately after end of its usage.  
    /// This method uses generic visitor pattern on template in order to gain vertex and edge attribute types which are then used in algorithms execution.  
    /// Provided user models and map representation must be tied to this template and its vertex/edge attribute types. If they are not, the exception is thrown.
    /// Execution of the algorithm can be adjusted by provided configuration.
    /// </summary>
    /// <param name="algorithm">Algorithm whose executor is provided.</param>
    /// <param name="mapRepre">Map representation upon which executor will look for paths.</param>
    /// <param name="userModel">User model which executor uses for path finding. Its associated template is used for defining vertex and edge attribute types used in path finding algorithm.</param>
    /// <param name="configuration">Configuration adjusting execution of the algorithm.</param>
    /// <returns>Executor of provided searching algorithm.</returns>
    /// <exception cref="ArgumentException">Thrown if eiter map representation is not graph or user model is not computing one or if graph is not tied to user models associated template.</exception>
    public  ISearchingExecutor GetExecutorOf(ISearchingAlgorithm algorithm, IMapRepre mapRepre, IUserModel<ITemplate> userModel, IConfiguration configuration)
    {
        return  userModel.AssociatedTemplate.AcceptGeneric<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration)>(this, (algorithm, mapRepre,userModel, configuration));
    }
    
    ISearchingExecutor ITemplateGenericVisitor<ISearchingExecutor, (ISearchingAlgorithm, IMapRepre,IUserModel<ITemplate>, IConfiguration)>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, (ISearchingAlgorithm, IMapRepre, IUserModel<ITemplate>, IConfiguration) otherParams)
    {
        var (algorithm, mapRepre, userModel, configuration) = otherParams;
        if (mapRepre is IGraph<IVertex, IEdge> graph)
            return graph.AcceptGeneric<ISearchingExecutor, TVertexAttributes, TEdgeAttributes, IVertexAttributes, IEdgeAttributes, (ISearchingAlgorithm, IUserModel<ITemplate>, IConfiguration)>(this, (algorithm, userModel, configuration));
        throw new ArgumentException("Provided map representation is not a graph.");
    }

    ISearchingExecutor IGraphGenericVisitor<ISearchingExecutor, IVertexAttributes, IEdgeAttributes, (ISearchingAlgorithm, IUserModel<ITemplate>, IConfiguration)>.
        GenericVisit<TGraph, TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(TGraph graph, (ISearchingAlgorithm, IUserModel<ITemplate>, IConfiguration) otherParams)
    {
        var (algorithm, userModel, configuration) = otherParams;
        if (userModel is IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> computingUserModel)
        {
                var executor = algorithm.GetExecutor(graph, computingUserModel, configuration);
                return executor;
        }
        throw new ArgumentException("Provided user model is not matching with vertex and edge attributes.");
    }
}





































// ITemplateGenericVisitor<HashSet<ISearchingAlgorithm>,IUserModelType<IUserModel<ITemplate>,ITemplate>>,

    /// <summary>
    /// Returns all algorithms that are able to use map representation type represented by provided representative.
    /// Map representation type must satisfy set of contracts that algorithm requires so it might work correctly.
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation type.</param>
    /// <returns>Set of usable algorithms for tested map representation type.</returns>
    // public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        // IMapRepreRepresentative<IMapRepre> mapRepreRep)
    // {
        // HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        // foreach (var searchingAlgorithm in SearchingAlgorithms)
        // {
            // if (searchingAlgorithm.DoesRepresentUsableMapRepre(mapRepreRep)) usableAlgorithms.Add(searchingAlgorithm);
        // }
        // return usableAlgorithms;
    // }

   
    /// <summary>
    /// Returns all algorithms that are able to use at least one map representation type represented by provided collection of representatives.
    /// It do it so by calling its overload <see cref="GetUsableAlgorithmsFor(Optepafi.Models.MapRepreMan.MapRepres.Representatives.IMapRepreRepresentative{Optepafi.Models.MapRepreMan.MapRepres.IMapRepre})"/> for each of provided representative and then joins results. 
    /// </summary>
    /// <param name="mapRepreReps">Representatives of tested map representations types.</param>
    /// <returns>Set of usable algorithms for set of tested map representation types.</returns>
    // public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        // IEnumerable<IMapRepreRepresentative<IMapRepre>> mapRepreReps)
    // {
        // HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        // foreach (var mapRepreRep in mapRepreReps)
        // {
            // usableAlgorithms.UnionWith(GetUsableAlgorithmsFor(mapRepreRep));
        // }
        // return usableAlgorithms;
    // }

    /// <summary>
    /// Checks if map representation type represented by provided representative is usable for provided searching algorithm.
    /// Map representation type must satisfy set of contracts that algorithm requires so it could work correctly.
    /// </summary>
    /// <param name="mapRepreRep">Representative of tested map representation type.</param>
    /// <param name="algorithm">Algorithm which tests map representation types usability.</param>
    /// <returns>True if map representation type is usable in provided algorithm. False otherwise.</returns>
    // public bool DoesRepresentUsableMapRepreFor(IMapRepreRepresentative<IMapRepre> mapRepreRep,
        // ISearchingAlgorithm algorithm)
    // {
        // return algorithm.DoesRepresentUsableMapRepre(mapRepreRep);
    // }

    // /// <summary>
    /// Returns all algorithms that are able to use user model type represented by provided representative.
    /// User model type must satisfy set of contracts that algorithm requires so it could work correctly.
    /// </summary>
    /// <param name="userModelType">Representative of tested user model type.</param>
    /// <returns>Set of usable algorithms for tested user model type.</returns>
    // public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    // {
        // return userModelType.AssociatedTemplate.AcceptGeneric<HashSet<ISearchingAlgorithm>, IUserModelType<IUserModel<ITemplate>, ITemplate>>(this, userModelType);
    // }
    // HashSet<ISearchingAlgorithm> ITemplateGenericVisitor<HashSet<ISearchingAlgorithm>, IUserModelType<IUserModel<ITemplate>, ITemplate>>.GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    // {
        // HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        // if (userModelType is IUserModelType< IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> computingUserModelType)
        // {
            // foreach (var searchingAlgorithm in SearchingAlgorithms)
            // {
                // if (searchingAlgorithm.DoesRepresentUsableUserModel(computingUserModelType))
                    // usableAlgorithms.Add(searchingAlgorithm);
            // }
        // }
        // return usableAlgorithms;
    // }

    /// <summary>
    /// Returns all algorithms that are able to use at least one user model type represented by provided collection of representatives.
    /// It do it so by calling its overload <see cref="GetUsableAlgorithmsFor(Optepafi.Models.UserModelMan.UserModelReps.IUserModelType{Optepafi.Models.UserModelMan.UserModels.IUserModel{Optepafi.Models.TemplateMan.ITemplate},Optepafi.Models.TemplateMan.ITemplate})"/> for each of provided representative and then joins results. 
    /// </summary>
    /// <param name="userModelTypes">Representatives of tested user model types.</param>
    /// <returns>Set of usable algorithms for set of tested user model types.</returns>
    // public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(IEnumerable<IUserModelType<IUserModel<ITemplate>, ITemplate>> userModelTypes)
    // {
        // HashSet<ISearchingAlgorithm> usableAlgorithms = new();
        // foreach (var userModelType in userModelTypes)
        // {
            // usableAlgorithms.UnionWith(GetUsableAlgorithmsFor(userModelType));
        // }
        // return usableAlgorithms;
    // }
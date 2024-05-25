using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public class SearchingAlgorithmManager : 
    ITemplateGenericVisitor<(SearchingAlgorithmManager.Result[], Path[]?[]),(Leg[], ISearchingAlgorithm, IMapRepre, IUserModel[], IProgress<ISearchingReport>?, CancellationToken?)>,
    ITemplateGenericVisitor<(SearchingAlgorithmManager.Result, ISearchingExecutor?), (ISearchingAlgorithm, IMapRepre, IUserModel)>
{
    public static SearchingAlgorithmManager Instance { get; } = new();
    private SearchingAlgorithmManager(){}

    public ISet<ISearchingAlgorithm> SearchingAlgorithms { get; } =
        ImmutableHashSet.Create<ISearchingAlgorithm>(/*TODO: doplnit algoritmy*/);

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

    public bool DoesRepresentUsableMapRepreFor(IMapRepreRepresentative<IMapRepre> mapRepreRep,
        ISearchingAlgorithm algorithm)
    {
        return algorithm.DoesRepresentUsableMapRepre(mapRepreRep);
    }
    
    public enum Result{Ok, NotComputingUserModel, NotUsableMapRepre }

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
            else results[i] = Result.NotComputingUserModel;
        }
        
        Path[][]? foundPaths;

        if (mapRepre is IGraph<TVertexAttributes, TEdgeAttributes> definedFunctionalityMapRepre)
            foundPaths = algorithm.ExecuteSearch(track, definedFunctionalityMapRepre, usableUserModels.ToArray(),
                progress, cancellationToken);
        else
            foundPaths = null;
        
        Path[]?[] resultingPaths = new Path[]?[userModels.Length];
        if (foundPaths is null)
        {
            for(int i = 0; i < userModels.Length; ++i)
            {
                if (results[i] != Result.NotComputingUserModel) results[i] = Result.NotUsableMapRepre;
            }
            Array.Fill(resultingPaths, null);
        }
        else
        {
            int j = 0;
            for (int i = 0; i < userModels.Length; ++i)
            {
                if (results[i] is not (Result.NotUsableMapRepre or Result.NotComputingUserModel))
                {
                    results[i] = Result.Ok;
                    resultingPaths[i] = foundPaths[j++];
                }
                else
                {
                    resultingPaths[i] = null;
                }
            }
        }
        return (results, resultingPaths);
    }

    public Result TryGetExecutorOf(ISearchingAlgorithm algorithm, IMapRepre mapRepre, IUserModel userModel, ITemplate template, out ISearchingExecutor? executor)
    {
        var (result, exec) = template.AcceptGeneric<(Result, ISearchingExecutor?), (ISearchingAlgorithm, IMapRepre, IUserModel)>(this, (algorithm, mapRepre,userModel));
        executor = exec;
        return result;
    }

    (Result, ISearchingExecutor?) ITemplateGenericVisitor<(Result, ISearchingExecutor?), (ISearchingAlgorithm, IMapRepre,IUserModel)>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, (ISearchingAlgorithm, IMapRepre, IUserModel) otherParams)
    {
        var (algorithm, mapRepre, userModel) = otherParams;
        if (userModel is IComputingUserModel<TVertexAttributes, TEdgeAttributes> computingUserModel)
        {
            if (mapRepre is IGraph<TVertexAttributes, TEdgeAttributes>
                definedFunctionalityMapRepre)
            {
                var executor = algorithm.GetExecutor(definedFunctionalityMapRepre, computingUserModel);
                return (executor is null ? Result.NotUsableMapRepre : Result.Ok, executor);
            }
            return (Result.NotUsableMapRepre, null);
        }
        return (Result.NotComputingUserModel, null);
    }
}
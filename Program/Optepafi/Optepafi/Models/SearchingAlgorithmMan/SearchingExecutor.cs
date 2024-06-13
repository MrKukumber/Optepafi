using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.ReportMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

/// <summary>
/// Represents searching executor that for provided track returns path found by associated searching algorithm. It executes path searching delegate on map representation by using one user model. These three sources should be provided to executor in its construction.
/// Executor provides way for executing searching on sequentially provided tracks whereby the searched graph does not have to be cleaned and returned to consistent state after each search.  
/// Principle of executor:
/// - executor is instantiated with graph, user model and delegate of searching algorithm
/// - during its construction, inner loop is activated which locks map representation for itself
/// - after that starts lifetime of executor
/// - synchronized requests for searching of path of provided tracks are accepted by method and passed to inner loop of executor
/// - inner loop will awake upon shifted request and will call delegate of searching algorithm to execute search for provided tracks in request on its locked graph
/// - when searching comes to the end, inner loop informs method which passed the request to it
/// - method will return the result of search and will let next synchronized request to be passed to inner loop
/// - at the end of the lifetime of executor it is disposed. That means it will stop the inner loop and by that will release its lock on graph. Before exiting it has to let graph to clean itself to restore its consistency.
/// - after this point executor can not be used again. Attempts of its use will end in throw of <c>ObjectDisposedException</c>.
///
/// This behaviour can be achieved by use of synchronization primitives.
/// It should be ensured that when executor is not be needed anymore it is disposed. The graph can then be released for further use. 
/// For its default implementation see <see cref="SearchingExecutor{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public interface ISearchingExecutor : IDisposable
{
    /// <summary>
    /// Method that can be called on executor and it will synchronously accept searching requests and pass them to inner loop of executor for resolution.
    /// </summary>
    /// <param name="track">Collection of legs for which paths should be searched for.</param>
    /// <param name="progress">Object by which can be progress of path finding subscribed.</param>
    /// <param name="cancellationToken">Token for search cancellation.</param>
    /// <returns>Collection of resulting found paths for legs of track.</returns>
    IPath[] Search(Leg[] track, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken);
}

/// <summary>
/// Implementation of <see cref="ISearchingExecutor"/>. It uses Mutex and AutoResetEvents for achieving correct behaviour of executor. 
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in edges of a graph.</typeparam>
public class SearchingExecutor<TVertexAttributes, TEdgeAttributes> :
    ISearchingExecutor
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public delegate IPath[] AlgorithmSearchingDelegate(
        Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> graph,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken);
    
    private readonly Mutex _searchMutex = new();
    private readonly AutoResetEvent _searchLoopResetEvent = new(false);
    private readonly AutoResetEvent _searchMethodResetEvent = new(false);
    private readonly AutoResetEvent _constructionResetEvent = new(false);
    private Leg[] _inputTrack = null;
    private IProgress<ISearchingReport>? _inputProgress;
    private CancellationToken? _inputCancellationToken;
    private IPath[] _outputPath = null;
    private bool _disposed = false;
    
    private readonly AlgorithmSearchingDelegate _algorithmSearchingDelegate;
    private readonly IComputingUserModel<TVertexAttributes, TEdgeAttributes> _userModel;
    private readonly IGraph<TVertexAttributes, TEdgeAttributes> _graph;
    

    public SearchingExecutor(IGraph<TVertexAttributes, TEdgeAttributes> graph, IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel, AlgorithmSearchingDelegate algorithmSearchingDelegate )
    {
        _graph = graph;
        _userModel = userModel;
        _algorithmSearchingDelegate = algorithmSearchingDelegate;
        Task.Run(ExecuteSearchingLoopAsync);
        _constructionResetEvent.WaitOne();
    }
    
    public IPath[] Search(Leg[] track, IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        if (_disposed) throw new ObjectDisposedException("SearchingExecutor");

        try { _searchMutex.WaitOne(); }
        catch (ObjectDisposedException) { throw new ObjectDisposedException("SearchingExecutor"); }

        _inputTrack = track;
        _inputProgress = progress;
        _inputCancellationToken = cancellationToken;
        
        if(!_searchLoopResetEvent.Set())  throw new ObjectDisposedException("SearchingExecutor");
        _searchMethodResetEvent.WaitOne();
        
        _searchMutex.ReleaseMutex();
        return _outputPath;
    }

    public void Dispose()
    {
        _searchMutex.WaitOne();
        _disposed = true;
        _searchLoopResetEvent.Set();
        
        _searchMutex.Dispose();
        _constructionResetEvent.Dispose();
        _searchLoopResetEvent.Dispose();
        _searchMethodResetEvent.Dispose();
    }
    private void ExecuteSearchingLoopAsync()
    {
        lock (_graph)
        {
            while(!_constructionResetEvent.Set()) Thread.Sleep(10);
            
            _searchLoopResetEvent.WaitOne();
            while (!_disposed)
            {
                _outputPath = _algorithmSearchingDelegate(_inputTrack, _graph, _userModel, _inputProgress, _inputCancellationToken);
                _searchMethodResetEvent.Set();
                _searchLoopResetEvent.WaitOne();
            }
            _graph.RestoreConsistency();
        }
    }
}
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public interface ISearchingExecutor : IDisposable
{
    Path[] Search(Leg[] track, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken);
}

public class SearchingExecutor<TVertexAttributes, TEdgeAttributes> :
    ISearchingExecutor
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public delegate Path[] AlgorithmSearchingDelegate(
        Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken);
    
    private readonly Mutex _searchMutex = new();
    private readonly AutoResetEvent _searchLoopResetEvent = new(false);
    private readonly AutoResetEvent _searchMethodResetEvent = new(false);
    private readonly AutoResetEvent _constructionResetEvent = new(false);
    private Leg[] _inputTrack;
    private IProgress<ISearchingReport>? _inputProgress;
    private CancellationToken? _inputCancellationToken;
    private Path[] _outputPath;
    private bool _disposed = false;
    
    private readonly AlgorithmSearchingDelegate _algorithmSearchingDelegate;
    private readonly IComputingUserModel<TVertexAttributes, TEdgeAttributes> _userModel;
    private readonly IGraph<TVertexAttributes, TEdgeAttributes> _mapRepre;
    

    public SearchingExecutor(
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre,
        IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        AlgorithmSearchingDelegate algorithmSearchingDelegate
        )
    {
        _mapRepre = mapRepre;
        _userModel = userModel;
        _algorithmSearchingDelegate = algorithmSearchingDelegate;
        Task.Run(ExecuteSearchingLoopAsync);
        _constructionResetEvent.WaitOne();
    }
    
    public Path[] Search(Leg[] track, IProgress<ISearchingReport>? progress = null, CancellationToken? cancellationToken = null)
    {
        if (_disposed) throw new ObjectDisposedException("SearchingExecutor");

        try
        {
            _searchMutex.WaitOne();
        }
        catch (ObjectDisposedException)
        {
            throw new ObjectDisposedException("SearchingExecutor");
        }

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
        lock (_mapRepre)
        {
            while(!_constructionResetEvent.Set()) Thread.Sleep(10);
            
            _searchLoopResetEvent.WaitOne();
            
            while (!_disposed)
            {
                _outputPath = _algorithmSearchingDelegate(_inputTrack, _mapRepre, _userModel, _inputProgress, _inputCancellationToken);
                _searchMethodResetEvent.Set();
                _searchLoopResetEvent.WaitOne();
            }
            _mapRepre.RestoreConsistency();
        }
    }
}
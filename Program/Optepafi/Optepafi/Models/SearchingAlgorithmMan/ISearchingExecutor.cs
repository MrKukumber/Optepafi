using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public interface ISearchingExecutor : IDisposable
{
    Path[] Search(Leg[] track);
}

public class SearchingExecutor<TVertexAttributes, TEdgeAttributes> :
    ISearchingExecutor
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    private Mutex searchMutex = new();
    private bool _disposed = false;
    
    private readonly AlgorithmSearchingDelegate _algorithmSearchingDelegate;
    private readonly IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> _userModel;
    private readonly IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> _mapRepre;
    
    public delegate Path[] AlgorithmSearchingDelegate(
        Leg[] track,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre
    );

    public SearchingExecutor(
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre,
        AlgorithmSearchingDelegate algorithmSearchingDelegate
        )
    {
        _userModel = userModel;
        _mapRepre = mapRepre;
        _algorithmSearchingDelegate = algorithmSearchingDelegate;
        Task.Run(ExecuteSearchingLoopAsync);
    }
    
    public Path[] Search(Leg[] track)
    {
        if (_disposed) throw new ObjectDisposedException("SearchingExecutor");
        
        searchMutex.WaitOne();
        try
        {

            //TODO: odovzdat trat loopu, ktory necha najst cestu a vrati ju v takom stave, aby ju tato metoda mohla vyzdvyhnut a vratit
            //      hladanie prebieha synchronizovane
        }
        finally
        {
            searchMutex.ReleaseMutex();
        }
    }

    private void ExecuteSearchingLoopAsync()
    {
        lock (_mapRepre)
        {
        //TODO: loop ktory bude zachytavat jednotlive zavolania Search metody a bude volat delegata algoritmu pre najdenie cesty
        //      treba vymysliet, ako sa bude tento loop zastavovat po disposenuti executoru
        //      tento loop bude drzat zamok na mapovej reprezentacii az po disposenutie executoru
        //      po zastaveni loopu nechat mapovu reprezentaciu vycistit a vratit do povodneho stavu
        }
    }

    public void Dispose()
    {
        //TODO: ukoncit loop a tym releasnut lock na mapovej reprezentacii
        searchMutex.Dispose();
        _disposed = true;
    }
}
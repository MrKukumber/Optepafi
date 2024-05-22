using System;
using System.Threading;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public interface ISearchingAlgoritmImplementation
{
    bool DoesRepresentUsableMapRepre(IGraphRepresentative<IGraph<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes>
            graphRepresentative);

    bool IsUsableMapRepre<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    Path[][] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre,
        IComputingUserModel<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>[] userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    sealed ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre,
        IComputingUserModel<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return new SearchingExecutor<TVertexAttributes, TEdgeAttributes>(mapRepre, userModel, ExecutorSearch);
    }

    protected Path[] ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IGraph<TVertexAttributes, TEdgeAttributes> mapRepre,
        IComputingUserModel<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;


}
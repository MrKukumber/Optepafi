using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public interface ISearchingAlgoritmImplementation
{
    bool DoesRepresentUsableMapRepre(IDefinedFunctionalityMapRepreRepresentativ<IDefinedFunctionalityMapRepre<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes>
            definedFunctionalityMapRepreRepresentativ);

    bool IsUsableMapRepre<TVertexAttributes, TEdgeAttributes>(
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    Path[][] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>[] userModels,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    sealed ISearchingExecutor GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
            userModel,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return new SearchingExecutor<TVertexAttributes, TEdgeAttributes>(userModel, mapRepre, ExecutorSearch);
    }

    protected Path[] ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;


}
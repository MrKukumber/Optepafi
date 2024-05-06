using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.SearchingAlgorithmMan;

public interface ISearchingAlgorithm
{
    string Name { get; }
    protected ISearchingAlgoritmImplementation[] Implementations { get; }

    sealed bool DoesRepresentUsableMapRepre(IMapRepreRepresentativ<IMapRepresentation> mapRepreRep)
    {
        var definedFunctionalityMapRepreRep = mapRepreRep.GetDefinedFunctionalityMapRepreRepresentative<IVertexAttributes, IEdgeAttributes>();
        foreach (var implementation in Implementations)
        {
            if (implementation.DoesRepresentUsableMapRepre(definedFunctionalityMapRepreRep))
                return true;
        }
        return false;
    }
    
    sealed Path[][]? ExecuteSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track,
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>[] userModels,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        lock (mapRepre)
        {
            foreach (var implementation in Implementations)
            {
                if (implementation.IsUsableMapRepre(mapRepre))
                {
                    return implementation.SearchForPaths(track, userModels, mapRepre);
                }
            }
            return default;
        }
    }

    sealed ISearchingExecutor? GetExecutor<TVertexAttributes, TEdgeAttributes>(
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes> mapRepre)
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var implementation in Implementations)
        {
            if (implementation.IsUsableMapRepre(mapRepre))
            {
                return implementation.GetExecutor(userModel, mapRepre);
            }
        }
        return default;
    }
        
    
    
    
}
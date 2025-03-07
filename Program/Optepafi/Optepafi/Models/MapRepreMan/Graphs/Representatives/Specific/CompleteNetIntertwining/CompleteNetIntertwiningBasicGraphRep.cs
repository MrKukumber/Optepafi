using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific.CompleteNetIntertwining;

public class CompleteNetIntertwiningBasicGraphRep<TVertexAttributes, TEdgeAttributes> :
    GraphRepresentative<ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttributes>, ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningBasicGraph<TVertexAttributes, TEdgeAttributes>.Edge>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
    public static CompleteNetIntertwiningBasicGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private CompleteNetIntertwiningBasicGraphRep() { }

    public override IImplementationIndicator<ITemplate, IMap, IMapRepre>[] CreateableImplementationsIndicators { get; } = [CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TTemplate, TVertexAttributes1, TEdgeAttributes1>(
        SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<TTemplate, TVertexAttributes1, TEdgeAttributes1>, TTemplate> userModelType, ISearchingAlgorithm algorithm)
        => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);
}
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

//TODO: comment
public class CompleteNetIntertwiningGraphRep<TVertexAttributes, TEdgeAttributes> : GraphRepresentative<ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>, ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<TVertexAttributes, TEdgeAttributes>.Edge>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static CompleteNetIntertwiningGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private CompleteNetIntertwiningGraphRep() { }
    
    public override IImplementationIndicator<ITemplate, IMap, IMapRepre>[] CreateableImplementationsIndicators { get; } = [CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TUserModelVertexAttributes, TUserModelEdgeAttributes>(SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<ITemplate<TUserModelVertexAttributes, TUserModelEdgeAttributes>, TUserModelVertexAttributes, TUserModelEdgeAttributes>, ITemplate<TUserModelVertexAttributes, TUserModelEdgeAttributes>> userModelType, ISearchingAlgorithm algorithm) => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);
}
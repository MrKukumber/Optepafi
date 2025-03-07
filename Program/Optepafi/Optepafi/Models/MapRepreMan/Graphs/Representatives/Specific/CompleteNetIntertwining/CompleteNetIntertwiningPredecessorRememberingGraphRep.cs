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

//TODO: comment
public class CompleteNetIntertwiningPredecessorRememberingGraphRep<TVertexAttributes, TEdgeAttributes> : 
    GraphRepresentative<ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>, ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<TVertexAttributes, TEdgeAttributes>.Edge>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static CompleteNetIntertwiningPredecessorRememberingGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private CompleteNetIntertwiningPredecessorRememberingGraphRep() { }
    
    public override IImplementationIndicator<ITemplate, IMap, IMapRepre>[] CreateableImplementationsIndicators { get; } = [CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TUserModelTemplate, TUserModelVertexAttributes, TUserModelEdgeAttributes>(SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<TUserModelTemplate, TUserModelVertexAttributes, TUserModelEdgeAttributes>, TUserModelTemplate> userModelType, ISearchingAlgorithm algorithm) 
        => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);
}
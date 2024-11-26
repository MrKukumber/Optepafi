using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

//TODO: comment
public class CompleteSnappingGraphRep : GraphRepresentative<ICompleteSnappingGraph, ICompleteSnappingGraph.Vertex<IVertexAttributes, IEdgeAttributes>, ICompleteSnappingGraph.Edge<IEdgeAttributes, IVertexAttributes>>
{
    public static CompleteSnappingGraphRep Instance { get; } = new();
    private CompleteSnappingGraphRep() { }
    
    public override IImplementationIndicator<ITemplate, IMap, ICompleteSnappingGraph>[] CreateableImplementationsIndicators { get; } = [CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TVertexAttributes, TEdgeAttributes>(SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType, ISearchingAlgorithm algorithm) => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);
}
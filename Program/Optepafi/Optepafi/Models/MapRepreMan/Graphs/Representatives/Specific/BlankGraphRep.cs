using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.Blank;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

/// <summary>
/// Singleton class which represents blank graph.
/// 
/// Its instance is contained in <c>BlankRepreRep</c> so it can be used for creation of represented map representation/graph. 
/// </summary>
public class BlankGraphRep : GraphRepresentative<IBlankGraph, IBlankGraph.Vertex<IVertexAttributes>, IBlankGraph.Edge<IEdgeAttributes>>
{
    public static BlankGraphRep Instance { get; } = new();
    private BlankGraphRep() { }
    
    ///<inheritdoc cref="GraphRepresentative{TGraph,TVertex,TEdge}.CreateableImplementationsIndicators"/>
    public override IImplementationIndicator<ITemplate, IMap, IBlankGraph>[] CreateableImplementationsIndicators { get; } = [BlankElevDataDepBlankTemplateTextMapImplementationRep.Instance, BlankElevDataIndepBlankTemplateOmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TVertexAttributes, TEdgeAttributes>(SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType, ISearchingAlgorithm algorithm) => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);

}
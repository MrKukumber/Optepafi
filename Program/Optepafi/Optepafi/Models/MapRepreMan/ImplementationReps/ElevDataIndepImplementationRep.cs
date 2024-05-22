using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public abstract class ElevDataIndepImplementationRep<TTemplate, TMap, TUsableSubMap,TGraph, TVertexAttributes, TEdgeAttributes> : 
    IImplementationIdentifier<TTemplate, TMap, TGraph>, IImplementationElevDataIndependentConstr<TTemplate,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TUsableSubMap : TMap, IMap
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = false;

    public abstract TGraph ConstructMapRepre(TTemplate template, TUsableSubMap map,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken);
}
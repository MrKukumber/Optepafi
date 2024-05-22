using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public abstract class ElevDataDepImplementationRep<TTemplate, TMap, TUsableSubMap, TGraph, TVertexAttributes, TEdgeAttributes> :
    IImplementationIdentifier<TTemplate, TMap, TGraph>, 
    IImplementationElevDataDependentConstr<TTemplate,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TUsableSubMap : TMap, IGeoLocatedMap
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = true;

    public abstract TGraph ConstructMapRepre(TTemplate template, TUsableSubMap map, IElevData elevData,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken);
}
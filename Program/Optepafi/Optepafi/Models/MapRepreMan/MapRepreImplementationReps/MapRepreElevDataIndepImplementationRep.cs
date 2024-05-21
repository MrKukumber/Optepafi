using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public abstract class MapRepreElevDataIndepImplementationRep<TTemplate, TMap, TUsableSubMap,TMapRepre, TVertexAttributes, TEdgeAttributes> : 
    IMapRepreImlpementationInfo<TTemplate, TMap, TMapRepre>, IElevDataIndependentConstr<TTemplate, TUsableSubMap, TMapRepre>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TUsableSubMap : TMap, IMap
    where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = false;

    public abstract TMapRepre ConstructMapRepre(TTemplate template, TUsableSubMap map,
        IProgress<MapRepreCreationReport>? progress, CancellationToken? cancellationToken);
}
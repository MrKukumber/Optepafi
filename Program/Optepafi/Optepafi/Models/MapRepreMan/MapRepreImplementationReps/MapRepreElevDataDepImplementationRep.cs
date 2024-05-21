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

public abstract class MapRepreElevDataDepImplementationRep<TTemplate, TMap, TConstrUsableSubMap, TMapRepre, TVertexAttributes, TEdgeAttributes> :
    IMapRepreImlpementationInfo<TTemplate, TMap, TMapRepre>, IElevDataDependentConstr<TTemplate, TConstrUsableSubMap, TMapRepre>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TConstrUsableSubMap : TMap, IGeoLocatedMap
    where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = true;

    public abstract TMapRepre ConstructMapRepre(TTemplate template, TConstrUsableSubMap map, IElevData elevData,
        IProgress<MapRepreCreationReport>? progress, CancellationToken? cancellationToken);
}
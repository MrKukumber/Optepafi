using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public interface IImplementationElevDataDependentConstr<in TTemplate, in TMap, out TGraph, TVertexAttributes, TEdgeAttributes> //: IMapRepreConstructor<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> where TMap : IMap where TGraph : IGraph<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public TGraph ConstructMapRepre(TTemplate template, TMap map, IElevData elevData, 
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken);

}
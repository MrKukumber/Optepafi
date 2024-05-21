using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreReps;

public interface IMapRepreRepresentativ<out TMapRepresentation> where TMapRepresentation : IMapRepresentation
{
    string MapRepreName { get; }

    //represents all map repre constructors, that returns TMapRepresentation
    IMapRepreImlpementationInfo<ITemplate, IMap, TMapRepresentation>[] MapRepreImplementationInfos { get; }

    IDefinedFunctionalityMapRepreRepresentativ<IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
        GetDefinedFunctionalityMapRepreRepresentative<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    sealed IMapRepresentation CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map,
        IProgress<MapRepreCreationReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetDefinedFunctionalityMapRepreRepresentative<TVertexAttributes, TEdgeAttributes>()
            .CreateMapRepre(template, map, progress, cancellationToken, MapRepreImplementationInfos);
    }

    sealed IMapRepresentation CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, IElevData elevData,
        IProgress<MapRepreCreationReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetDefinedFunctionalityMapRepreRepresentative<TVertexAttributes, TEdgeAttributes>()
            .CreateMapRepre(template, map, elevData, progress, cancellationToken, MapRepreImplementationInfos);
    }
}

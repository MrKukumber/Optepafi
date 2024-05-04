using System;
using System.Threading;
using Avalonia.Controls;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreRepres;

public interface IMapRepreRepresentativ<out TMapRepresentation> where TMapRepresentation : IMapRepresentation
{
    string MapRepreName { get; }

    //represents all map repre constructors, that returns TMapRepresentation
    IMapRepreConstructor<ITemplate, IMap, TMapRepresentation>[] MapRepreConstrs { get; }

    IDefinedFunctionalityMapRepreRepresentativ<IMapRepreWithDefinedFunctionality<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate, TVertexAttributes, TEdgeAttributes>
        GetDefinedFunctionalityMapRepreRepresentative<TTemplate, TVertexAttributes, TEdgeAttributes>()
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    sealed IMapRepresentation? CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetDefinedFunctionalityMapRepreRepresentative<TTemplate, TVertexAttributes, TEdgeAttributes>()
            .CreateMapRepre(template, map, (IMapRepreRepresentativ<IMapRepresentation>)this, progress, cancellationToken, MapRepreConstrs);
    }

    sealed IMapRepresentation? CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, ElevData elevData,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetDefinedFunctionalityMapRepreRepresentative<TTemplate, TVertexAttributes, TEdgeAttributes>()
            .CreateMapRepre(template, map, elevData, (IMapRepreRepresentativ<IMapRepresentation>)this, progress, cancellationToken, MapRepreConstrs);
    }
}

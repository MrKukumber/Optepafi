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

public interface IMapRepreRepresentative<out TMapRepresentation> where TMapRepresentation : IMapRepre
{
    string MapRepreName { get; }

    //represents all implementations, that returns TMapRepresentation
    IImplementationIdentifier<ITemplate, IMap, TMapRepresentation>[] ImplementationIdentifiers { get; }

    IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
        GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    sealed IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
            .CreateGraph(template, map, progress, cancellationToken, ImplementationIdentifiers);
    }

    sealed IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, IElevData elevData,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    {
        return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
            .CreateGraph(template, map, elevData, progress, cancellationToken, ImplementationIdentifiers);
    }
}

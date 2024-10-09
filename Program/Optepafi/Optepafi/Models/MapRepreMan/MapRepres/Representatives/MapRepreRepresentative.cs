using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives;

public abstract class MapRepreRepresentative<TMapRepre, TConfiguration> : IMapRepreRepresentative<TMapRepre>
    where TMapRepre : IMapRepre
    where TConfiguration : IConfiguration
{
    public abstract string MapRepreName { get; }
    public abstract IImplementationIndicator<ITemplate, IMap, TMapRepre>[] ImplementationIndicators { get; }
    public abstract IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    protected abstract TConfiguration DefaultConfiguration { get; }
    public IConfiguration DefaultConfigurationDeepCopy => DefaultConfiguration.DeepCopy(); 

    public IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, IConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    { 
        if (configuration is TConfiguration config) 
            return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
                .CreateGraph(template, map, config, progress, cancellationToken, ImplementationIndicators);
        // TODO: log wrong type of retrieved configuration
        return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
            .CreateGraph(template, map, DefaultConfiguration, progress, cancellationToken, ImplementationIndicators);
    }
    public IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, IElevData elevData, IConfiguration configuration, 
            IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
            where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
            where TMap : IGeoLocatedMap 
            where TVertexAttributes : IVertexAttributes 
            where TEdgeAttributes : IEdgeAttributes
        { 
            if (configuration is TConfiguration config) 
                return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
                    .CreateGraph(template, map, elevData, config, progress, cancellationToken, ImplementationIndicators); 
            // TODO: log wrong type of retrieved configuration
            return GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>()
                .CreateGraph(template, map, elevData, DefaultConfiguration, progress, cancellationToken, ImplementationIndicators); 
        }
}
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives;

//TODO: comment
public abstract class MapRepreRepresentative<TMapRepre, TConfiguration> : IMapRepreRepresentative<TMapRepre>
    where TMapRepre : IMapRepre
    where TConfiguration : IConfiguration
{
    public abstract string MapRepreName { get; }
    public abstract IGraphCreator<TMapRepre>[] GetGraphCreators<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
    
    public IImplementationIndicator<ITemplate, IMap, IMapRepre>[] ImplementationIndicators => GetGraphCreators<IVertexAttributes, IEdgeAttributes>().SelectMany(x => x.CreateableImplementationsIndicators).ToArray();
    protected abstract TConfiguration DefaultConfiguration { get; }
    public IConfiguration DefaultConfigurationDeepCopy => DefaultConfiguration.DeepCopy(); 

    public IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, int graphCreatorIndex, 
        IConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TVertexAttributes : IVertexAttributes 
        where TEdgeAttributes : IEdgeAttributes
    { 
        if (graphCreatorIndex < 0 || graphCreatorIndex >= GetGraphCreators<TVertexAttributes, TEdgeAttributes>().Count())
            throw new IndexOutOfRangeException(nameof(graphCreatorIndex));
        if (configuration is TConfiguration config) 
            return GetGraphCreators<TVertexAttributes, TEdgeAttributes>()[graphCreatorIndex]
                .CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(template, map, config, progress, cancellationToken);
        // TODO: log wrong type of retrieved configuration
        Console.WriteLine($"Wrong type of retrieved configuration in {typeof(TMapRepre).Name} representative. Retrived confifiguration was of type {configuration.GetType().Name}.");
        return GetGraphCreators<TVertexAttributes, TEdgeAttributes>()[graphCreatorIndex]
            .CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(template, map, DefaultConfiguration, progress, cancellationToken);
    }
    public IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, int graphCreatorIndex,
        IElevData elevData, IConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
            where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
            where TMap : IGeoLocatedMap 
            where TVertexAttributes : IVertexAttributes 
            where TEdgeAttributes : IEdgeAttributes
    {
        if (graphCreatorIndex < 0 || graphCreatorIndex >= GetGraphCreators<TVertexAttributes, TEdgeAttributes>().Count())
            throw new IndexOutOfRangeException(nameof(graphCreatorIndex));
        if (configuration is TConfiguration config) 
            return GetGraphCreators<TVertexAttributes, TEdgeAttributes>()[graphCreatorIndex]
                .CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(template, map, elevData, config, progress, cancellationToken); 
        // TODO: log wrong type of retrieved configuration
        Console.WriteLine($"Wrong type of retrieved configuration in {typeof(TMapRepre).Name} representative. Retrived confifiguration was of type {configuration.GetType().Name}.");
        return GetGraphCreators<TVertexAttributes, TEdgeAttributes>()[graphCreatorIndex]
            .CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(template, map, elevData, DefaultConfiguration, progress, cancellationToken); 
    }
}
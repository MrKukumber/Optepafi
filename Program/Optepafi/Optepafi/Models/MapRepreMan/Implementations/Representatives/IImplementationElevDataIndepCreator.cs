using System;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives;

/// <summary>
/// Represents constructor of graphs implementation which uses defined template and map types for its construction and does not need elevation data for it.
/// It is used by graph representative for construction of specific implementation of graph.  
/// This interface should not be implemented right away. Preferred way is to derive either from <see cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertexAttributes,TEdgeAttributes}"/> class or from <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertexAttributes,TEdgeAttributes}"/> class.  
/// Thanks to contravariant type parameters for template and map it is suitable for pattern matching and identifying correct template and map types combinations.  
/// </summary>
/// <typeparam name="TTemplate">Type of template used in implementation and its construction.</typeparam>
/// <typeparam name="TMap">Type of map used in implementation and its construction.</typeparam>
/// <typeparam name="TGraph">Type of graph that is returned from construction.</typeparam>
/// <typeparam name="TConfiguration">Type of configuration used in graph construction process. It is set by graph representative so that all implementations use same configuration type.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in vertices of returned graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in edges of returned graph.</typeparam>
public interface IImplementationElevDataIndepCreator<in TTemplate, in TMap, out TGraph, in TConfiguration, out TVertex, out TEdge, out TVertexAttributes, out TEdgeAttributes> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TMap : IMap 
    where TGraph : IGraph<TVertex, TEdge>
    where TConfiguration : IConfiguration
    where TVertex : IVertex
    where TEdge : IEdge
    where TVertexAttributes : IVertexAttributes 
    where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Method for construction of represented implementation without use of elevation data.
    /// </summary>
    /// <param name="template">Template used in construction of implementation.</param>
    /// <param name="map">Map used in construction of implementation</param>
    /// <param name="configuration">Configuration of created map representation.Type of configuration is set by graph representative so that all implementations use same configuration type.</param>
    /// <param name="progress">Object by which can be progress of construction subscribed.</param>
    /// <param name="cancellationToken">Token for cancelling of construction.</param>
    public TGraph CreateImplementation(TTemplate template, TMap map, TConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
    
}
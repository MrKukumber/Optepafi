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
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives;

/// <summary>
/// Represents representative of map representation :).
/// 
/// It contains:
/// 
/// - methods for creating map representations that it represents
/// - collection of implementation indicators
/// - reference to the corresponding graph representative
///
/// Implementation indicator collection should contain either <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}"/>. or <see cref="MapRepreManager"/> instances so they could be used for map creation too.  
/// Each implementation representative should occur in this collection for one template-map combination at most once. So there should be at most one elev data dependent and at most one elev data independent implementation for each template-map combination.  
/// Corresponding graph representative provides work with corresponding graph derived from represented map representation by implementation of this interface.  
/// Each map representation should have its own representative, so it could be presented at <see cref="MapRepreManager"/> as viable map representation.  
/// Preferred way to interact with representatives is through <see cref="MapRepreManager"/>.  
/// </summary>
/// <typeparam name="TMapRepre">Type of represented map representation.</typeparam>
public interface IMapRepreRepresentative<out TMapRepre> where TMapRepre : IMapRepre
{
    string MapRepreName { get; }

    /// <summary>
    /// Indicator collection of all implementations of represented map representation.
    /// 
    /// These indicators should be of <see cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}"/> or <see cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertexAttributes,TEdgeAttributes}"/> type so they could be used for map creation too.
    /// </summary>
    IImplementationIndicator<ITemplate, IMap, IMapRepre>[] ImplementationIndicators { get; }
    
    /// <summary>
    /// Returns collection of graph creators which construct corresponding graphs derived from represented map representation.
    /// These creators should be in collection sorted in a way such that creators which creates the fastest and the lightest
    /// graphs are at the beginning, and creators which creates the heaviest and the slowest ones are at the end.
    /// </summary>
    /// <returns>Creator of corresponding graph to the map representation.</returns>
    IGraphCreator<TMapRepre>[] GetGraphCreators<TVertexAttributes, TEdgeAttriubtes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttriubtes : IEdgeAttributes;

    /// <summary>
    /// Default configuration of represented map representation.
    /// </summary>
    IConfiguration DefaultConfigurationDeepCopy { get; }

    /// <summary>
    /// Method which creates map representation from provided template and map represented by this representative.
    /// 
    /// It constructs the map representation without requiring elevation data.  
    /// It calls for graph creation on corresponding graph representative. This created graph inherits from map repre. represented by this representative.  
    /// Before calling this method it should be checked that this representative contains constructor for provided template and map and it does not require elevation data. Construction may throw exception if provided combination id not usable.  
    /// </summary>
    /// <param name="template">Used template in map representation creation.</param>
    /// <param name="map">Used map in map representation creation.</param>
    /// <param name="graphCreatorIndex">Index of graph constructor, which is used for creation of graph reprezentation of map.</param>
    /// <param name="configuration">Configuration of created map representation. Type of configuration must match with <c>TConfiguration</c> type parameter of corresponding graph representative.</param>
    /// <param name="progress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <typeparam name="TTemplate">Type of provided template. Used for finding of corresponding constructor.</typeparam>
    /// <typeparam name="TMap">Type of provided map. Used for finding of corresponding constructor.</typeparam>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes represented by provided template.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes represented by provided template.</typeparam>
    /// <returns>Created map representation.</returns>
    IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, int graphCreatorIndex, 
        IConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;

    /// <summary>
    /// Method which creates map representation from provided template and map represented by this representative.
    /// For construction of this map representation are required elevation data for provided maps area.
    /// It calls for graph creation on corresponding graph representative. This created graph inherits form map repre. represented by this representative.
    /// Before calling this method it should be checked that given representative contains constructor for provided template and map and that it requires elevation data. Construction may throw exception if provided combination is not usable.
    /// </summary>
    /// <param name="template">Used template in map representation creation.</param>
    /// <param name="map">Used map in map representation creation.</param>
    /// <param name="graphCreatorIndex">Index of graph constructor, which is used for creation of graph reprezentation of map.</param>
    /// <param name="elevData">Elevation data corresponding to map area used in map representation creation.</param>
    /// <param name="configuration">Configuration of created map representation. Type of configuration must match with <c>TConfiguration</c> type parameter of corresponding graph representative.</param>
    /// <param name="progress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <typeparam name="TTemplate">Type of provided template. Used for finding of corresponding constructor.</typeparam>
    /// <typeparam name="TMap">Type of provided map. Used for finding of corresponding constructor.</typeparam>
    /// <typeparam name="TVertexAttributes">Type of vertex attributes represented by provided template.</typeparam>
    /// <typeparam name="TEdgeAttributes">Type of edge attributes represented by provided template.</typeparam>
    /// <returns>Created map representation.</returns>
    IMapRepre CreateMapRepre<TTemplate, TMap, TVertexAttributes, TEdgeAttributes>(TTemplate template, TMap map, int graphCreatorIndex, 
        IElevData elevData, IConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IGeoLocatedMap
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}
